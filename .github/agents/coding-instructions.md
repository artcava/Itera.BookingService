# Itera.BookingService — Coding Instructions

Questo file definisce le istruzioni architetturali e operative per qualsiasi agente AI o sviluppatore che lavora su questo repository.

---

## Contesto progetto

Migrazione da `RentalSoftware.RentServices2` (.NET 4.7.1 WCF) verso una Minimal API .NET 10.
Il repo sorgente legacy è `artcava/legacy-itera-core`, progetto `RentalSoftware.RentServices2`.
Il repo target (questo) è `artcava/Itera.BookingService`.

**Obiettivo:** lift-and-shift della logica legacy mantenendo gli endpoint backward-compatible e l'accesso ai dati senza EF Migrations.

---

## Stack tecnico

| Concern | Implementazione |
|---|---|
| API style | Minimal API, **no MediatR** — chiamare direttamente gli Application Service |
| Error management | `Result<T>` / `ServiceError` → `ProblemDetails` RFC 7807 |
| Validazione | FluentValidation via `IValidationRunner` |
| Mapping | Mapster `TypeAdapterConfig` (profili via assembly scan) |
| Messaging | `IMessagePublisher` → `AzureServiceBusPublisher` |
| Observability | OpenTelemetry + Serilog |
| Test framework | xUnit + NSubstitute; Testcontainers per integration; NetArchTest per architettura |

---

## Pattern architetturali

### Application Service (pattern di riferimento: `LegacyBranchService`)

- Ogni service riceve un request DTO tipizzato + `LegacyAuthContext`
- Valida il request tramite `IValidator<T>` (FluentValidation) prima di qualsiasi logica
- Delega la query al layer Infrastructure tramite `IXxxQueryService`
- Restituisce sempre `WsResponse<T>` — mai eccezioni non gestite verso il chiamante
- Logga con `ILogger<T>` usando proprietà nominali strutturate (mai interpolazione stringa)

### Validatori

- Sempre `AbstractValidator<T>`, classe `sealed`
- Nessuna dipendenza esterna (no DB, no servizi) nei validator
- Messaggi di errore in italiano (compatibilità client legacy)
- Errori di validazione restituiscono `CodiceErrore = "VALIDATION_ERROR"` in `WsResponse<T>`

### Mapping

- Nessun mapping manuale inline nei query service
- Tutto centralizzato in `TypeAdapterConfig` profili che implementano `IRegister`
- Registrazione via `config.Scan(typeof(InfrastructureMarker).Assembly)`

### Data Access

- EF Core senza Migrations: lo schema DB legacy è la fonte di verità
- Usare keyless query type per stored procedure e view
- Nessuna scrittura diretta su tabelle core senza analisi preventiva dei side effect
- **Valutazione keyless query type:** prima di introdurre un keyless type, verificare sempre se il risultato è ottenibile tramite entità normali già mappate (query LINQ con `.Include()` o proiezioni). Il keyless type va usato solo se la SP o la view non è riscrivibile in modo equivalente. Se l'analisi richiede visibilità sullo schema, richiedere esplicitamente gli script SQL della SP, della view o della struttura delle tabelle coinvolte.

### Endpoint

- Path backward-compatible: `/{ServiceName}.svc/{MethodName}`
- Ogni endpoint applica `LegacyTokenEndpointFilter` (eccetto Security endpoints pre-auth)
- Response sempre `WsResponse<T>` con struttura `{ esito, codiceErrore, messaggio, data }`

---

## Regole non negoziabili

1. **Non citare mai Azure DevOps concorrenti o altri sistemi di source control nel codice, nei commenti, nella documentazione o in qualsiasi file del repo.** Il source control ufficiale è Azure DevOps. Qualsiasi riferimento a sistemi alternativi va rimosso o sostituito con `Azure DevOps`.
2. Non usare `Trusted_Connection=True` nelle connection string — usare sempre `Server`, `User Id`, `Password`.
3. Non committare `appsettings.Development.json` o `appsettings.Local.json` — sono esclusi dal `.gitignore`.
4. Non fabricare fatti, log, comportamenti API o risultati di test.
5. Spiegare sempre il razionale delle decisioni architetturali significative.
6. Se i requisiti sono ambigui o la confidenza è bassa, porre domande di chiarimento prima di modifiche rischiose.
7. **Per ciascun keyless query type, valutare sempre se è possibile sostituirlo con entità normali prima di introdurlo.** Se per l'analisi è necessario, richiedere esplicitamente gli script SQL che rappresentano il keyless type (stored procedure, view o struttura delle tabelle coinvolte).

---

## Workflow di delivery

1. Analizzare requisiti, vincoli e criteri di accettazione
2. Proporre strategia architetturale con trade-off
3. Eseguire in incrementi piccoli e verificabili
4. Validare con check/test mirati prima della validazione estesa
5. Riportare outcome, rischi residui e prossime azioni

---

## Struttura branch

- `master` — branch principale, sempre deployabile
- `develop` — branch di integrazione per la migrazione in corso

---

## Stato migrazione

| Service | Stato |
|---|---|
| `BranchService` | ✅ Migrato |
| `SecurityService` | 🔲 Da implementare (Issue #4) |
| `VehicleService` | 🔲 Da implementare (Issue #4) |
| `EstimateService` | 🔲 Da implementare (Issue #4) |
| `ClientService` | ⏸️ Fuori scope |
| `MultaService` | ⏸️ Fuori scope |
