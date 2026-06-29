using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Shared;
using Itera.BookingService.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Infrastructure.Security;

public sealed class SecurityQueryService : ISecurityQueryService
{
    private readonly IteraDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<SecurityQueryService> _logger;

    public SecurityQueryService(
        IteraDbContext db,
        IMapper mapper,
        ILogger<SecurityQueryService> logger)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> AuthenticateAsync(
        LoginRequest request, CancellationToken ct)
    {
        // La password legacy è hashata con MD5 uppercase hex — replica fedele.
        var passwordHash = ComputeMd5Hash(request.Password);

        var user = await _db.WsUsers
            .AsNoTracking()
            .Include(u => u.WsUserGruppos)
            .Where(u => u.Username == request.Username
                     && u.Password == passwordHash
                     && u.CodiceFiliale == request.CodiceFiliale
                     && u.Attivo == true)
            .FirstOrDefaultAsync(ct);

        if (user is null)
        {
            _logger.LogWarning("Autenticazione fallita per Username={Username}", request.Username);
            return Result<LoginResponse>.Failure(
                new ServiceError("AUTH_FAILED", "Credenziali non valide o utente non attivo."));
        }

        var token = GenerateLegacyToken(user.UserId, user.CodiceFiliale);
        var gruppi = user.WsUserGruppos.Select(g => g.CodiceGruppo).ToList();

        var response = new LoginResponse(
            Token: token,
            UserId: user.UserId,
            Username: user.Username,
            NomeCompleto: $"{user.Nome} {user.Cognome}".Trim(),
            CodiceFiliale: user.CodiceFiliale,
            Gruppi: gruppi);

        return Result<LoginResponse>.Success(response);
    }

    public async Task<Result<WsUserDto>> GetUserInfoAsync(
        string userId, CancellationToken ct)
    {
        var user = await _db.WsUsers
            .AsNoTracking()
            .Include(u => u.WsUserGruppos)
            .Include(u => u.WsUserListinos)
                .ThenInclude(ul => ul.Listino)
            .Where(u => u.UserId == userId)
            .FirstOrDefaultAsync(ct);

        if (user is null)
            return Result<WsUserDto>.Failure(
                new ServiceError("USER_NOT_FOUND", "Utente non trovato."));

        var dto = _mapper.Map<WsUserDto>(user);
        return Result<WsUserDto>.Success(dto);
    }

    // ---------------------------------------------------------------
    // Helpers privati
    // ---------------------------------------------------------------

    private static string ComputeMd5Hash(string input)
    {
        var bytes = System.Security.Cryptography.MD5.HashData(
            System.Text.Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes); // uppercase, come legacy
    }

    /// <summary>
    /// Replica il token legacy: base64(userId|codiceFiliale|timestamp).
    /// I client esistenti si aspettano questo formato.
    /// </summary>
    private static string GenerateLegacyToken(string userId, string codiceFiliale)
    {
        var raw = $"{userId}|{codiceFiliale}|{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(raw));
    }
}