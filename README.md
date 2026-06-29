# Itera.BookingService

Minimal API .NET 10 — migrazione da `RentalSoftware.RentServices2` (.NET 4.7.1 WCF).

Gli endpoint sono **backward-compatible** con il contratto legacy WCF: stessi path `.svc`, stessa struttura `WsResponse<T>`, stesso meccanismo di autenticazione token.

---

## Architettura

```
src/
  Itera.BookingService.Api/           # Minimal API — endpoint, filtri, Program.cs
  Itera.BookingService.Application/   # Application services, validatori FluentValidation
  Itera.BookingService.Infrastructure/ # EF Core (no Migrations), query service, auth
  Itera.BookingService.Contracts/     # DTO legacy (WsResponse<T>, WsFiliale, ...)
tests/
  Itera.BookingService.Api.Tests/     # xUnit — Integration + Unit
```

### Layer

| Layer | Responsabilità |
|---|---|
| **Api** | Routing Minimal API, `LegacyTokenEndpointFilter`, Swagger |
| **Application** | `ILegacyXxxService`, validazione `FluentValidation`, logica di business |
| **Infrastructure** | `LegacyDbContext` (EF Core, no Migrations), query service, `LegacyTokenValidationService` |
| **Contracts** | `WsResponse<T>`, DTO `Ws*`, `LegacyAuthContext`, `LegacyErrorCodes` |

---

## Setup locale

### Prerequisiti

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (locale o remoto) con il DB `RentalSoftware` legacy
- [VS Code](https://code.visualstudio.com/) con estensione [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

### Configurazione

1. Clonare il repo da Azure DevOps e posizionarsi nella root:
   ```bash
   git clone <AZURE_DEVOPS_REPO_URL>
   cd Itera.BookingService
   ```

2. Copiare il template di configurazione locale:
   ```bash
   cp src/Itera.BookingService.Api/appsettings.Development.json.template \
      src/Itera.BookingService.Api/appsettings.Development.json
   ```

3. Aprire `appsettings.Development.json` e sostituire i placeholder con i valori del DB di sviluppo:
   ```json
   "LegacyMain": "Server=<DEV_SERVER>;Database=RentalSoftware;User Id=<USERNAME>;Password=<PASSWORD>;TrustServerCertificate=True"
   ```

   > ⚠️ `appsettings.Development.json` è escluso dal `.gitignore` — non committarlo mai.

4. Avviare l'API in VS Code premendo `F5` (configurazione `C#: Itera.BookingService (http)`).
   Il browser si aprirà automaticamente su `http://localhost:5209/swagger`.

### Variabili d'ambiente in produzione

In produzione le credenziali vengono iniettate tramite environment variables (Azure App Service / container):

| Variabile | Descrizione |
|---|---|
| `ConnectionStrings__LegacyMain` | Connection string SQL Server (`Server=...;Database=...;User Id=...;Password=...`) |
| `ApplicationInsights__ConnectionString` | Stringa di connessione Application Insights |
| `LegacyAuth__TokenValidPeriodHours` | Durata token in ore (default: `24`) |
| `LegacyInfrastructure__EnableDetailedErrors` | `false` in produzione |

---

## Eseguire i test

```bash
dotnet test
```

I test di integrazione usano `BookingApiFactory` (WebApplicationFactory) con mock dei query service — non richiedono un DB reale.

---

## Stato migrazione

| Service | Endpoint | Stato |
|---|---|---|
| `BranchService` | `GetAllBranches`, `GetInfoBranch` | ✅ Migrato |
| `SecurityService` | `GetToken`, `ValidateToken`, `ResetKeyCache` | 🔲 In lavorazione |
| `VehicleService` | `GetVehicle` | 🔲 In lavorazione |
| `EstimateService` | 13 endpoint | 🔲 In lavorazione |
| `ClientService` | 17 endpoint | ⏸️ Fuori scope |
| `MultaService` | `NotificaLavorato`, `ConfermaLavorato` | ⏸️ Fuori scope |

---

## Decisioni architetturali

- **No EF Migrations**: lo schema del DB legacy è la fonte di verità. EF Core è usato in modalità read-only sullo schema esistente tramite keyless query type e stored procedure.
- **No MediatR**: i service applicativi vengono chiamati direttamente dagli endpoint Minimal API.
- **WsResponse\<T\> compat**: tutte le risposte mantengono la struttura `{ esito, codiceErrore, messaggio, data }` del contratto WCF legacy.
- **Autenticazione token**: il token viene letto dall'header `X-Api-Token` o, come fallback, dal body JSON del request (`EnablePayloadTokenFallback`).
- **SQL originali**: le stored procedure legacy sono archiviate in `docs/sql/` a scopo documentale.
