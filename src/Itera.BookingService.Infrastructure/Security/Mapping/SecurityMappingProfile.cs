using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Infrastructure.Persistence.Entities;
using Mapster;

namespace Itera.BookingService.Infrastructure.Security.Mapping;

public sealed class SecurityMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WsUser, WsUserDto>()
            .Map(dest => dest.NomeCompleto,
                 src => $"{src.Nome} {src.Cognome}".Trim())
            .Map(dest => dest.Gruppi, src => src.WsUserGruppos)
            .Map(dest => dest.Listini, src => src.WsUserListinos);

        config.NewConfig<WsUserGruppo, WsUserGruppoDto>();

        config.NewConfig<WsUserListino, WsUserListinoDto>()
            .Map(dest => dest.CodiceListino,   src => src.Listino.Codice)
            .Map(dest => dest.DescrizioneListino, src => src.Listino.Descrizione);
    }
}