# Skyworx Credit Application API

Backend technical test implementation for a Credit Application Management API.

## Tech Stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- Npgsql
- JWT Authentication
- Serilog
- xUnit + FluentAssertions + Moq
- Swagger / OpenAPI
- GitHub Actions CI

## Architecture

The solution is separated into:

- `SkyworxCredit.Domain` — domain entity
- `SkyworxCredit.Application` — DTOs, validation, business logic and interfaces
- `SkyworxCredit.Infrastructure` — EF Core DbContext, migrations and repository
- `SkyworxCredit.Api` — controllers, authentication, middleware and API configuration
- `SkyworxCredit.Tests` — unit tests

## Configuration

Secrets are intentionally not stored in source control.

Copy the values from `ENVIRONMENT.example.ps1` and replace placeholders with local values.

PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Database=skyworx_credit;Username=postgres;Password=<YOUR_POSTGRES_PASSWORD>"
$env:Jwt__Key = "<GENERATE_A_LONG_RANDOM_SECRET>"
$env:Auth__Username = "admin"
$env:Auth__Password = "<SET_A_DEMO_PASSWORD>"
```

The same environment variables are used by the EF Core design-time DbContext factory.

## Optional Token Helper

`get_token.py` can request a JWT without storing a password in source code:

```powershell
python get_token.py <username> <password> [base_url]
```

## Database

The requested database object is `pengajuan_kredit` with:

- `id` UUID primary key
- `plafon` numeric
- `bunga` decimal(5,2)
- `tenor` integer
- `angsuran` numeric
- `created_at` timestamp with time zone
- `updated_at` timestamp with time zone
- indexes on `plafon` and `tenor`

SQL files are provided in `database/`.

EF Core migrations are provided in:

`src/SkyworxCredit.Infrastructure/Migrations/`

### Apply migrations

Set the connection string first, then:

```powershell
dotnet ef database update --project src/SkyworxCredit.Infrastructure --startup-project src/SkyworxCredit.Api
```

## Run

```powershell
dotnet restore
dotnet build
dotnet run --project src/SkyworxCredit.Api/SkyworxCredit.Api.csproj
```

Open the Swagger URL printed by the application console.

## Authentication

`POST /api/Auth/login`

Example:

```json
{
  "username": "admin",
  "password": "<SET_A_DEMO_PASSWORD>"
}
```

Use the returned JWT as a Bearer token in Swagger for protected endpoints.

## Main Endpoints

### Authentication

- `POST /api/Auth/login`

### Credit Application CRUD

- `POST /api/PengajuanKredit`
- `GET /api/PengajuanKredit`
- `GET /api/PengajuanKredit/{id}`
- `PUT /api/PengajuanKredit/{id}`
- `DELETE /api/PengajuanKredit/{id}`

### Business Queries

- `POST /api/PengajuanKredit/hitung-angsuran`
- `GET /api/PengajuanKredit/longest-highest`
- `GET /api/PengajuanKredit/average-bunga`

## Example Request

```json
{
  "plafon": 100000000,
  "bunga": 12,
  "tenor": 60
}
```

For the example above, the annuity calculation returns approximately:

- Monthly installment: `2,224,444.77`
- Total payment: `133,466,686.20`
- Total interest: `33,466,686.20`

## Validation

The credit request validates:

- `plafon > 0`
- `tenor > 0`
- `bunga` between `0` and `100`

Invalid input returns HTTP 400.

Missing credit data returns HTTP 404.

Unhandled exceptions are handled by global middleware and logged with Serilog.

## Testing

The project contains unit tests for:

- Credit application creation
- Get by ID
- Get all
- Update
- Delete
- Installment calculation
- Zero-interest calculation
- Large-value calculation
- Validation scenarios

Verified locally:

```text
Passed! - Failed: 0, Passed: 16, Skipped: 0, Total: 16
```

## Performance & Scalability

For production scale:

1. Keep database queries asynchronous and use `AsNoTracking()` for read-only workloads where appropriate.
2. Add pagination to list endpoints.
3. Maintain indexes based on actual query patterns.
4. Use Redis for frequently requested, slowly changing reference/query data.
5. Add distributed tracing, metrics and centralized logs.
6. Scale API instances horizontally behind a load balancer.
7. Use database connection pooling and tune PostgreSQL based on workload.
8. Apply rate limiting and request-size limits at the API/gateway layer.

## Enterprise System Design

A production architecture can use:

`Client -> API Gateway/Load Balancer -> Stateless .NET API -> PostgreSQL`

                    ┌─────────────────────┐
                    │       Client        │
                    │ Web / Mobile / App  │
                    └──────────┬──────────┘
                               │ HTTPS
                               ▼
                    ┌─────────────────────┐
                    │   API Gateway / WAF │
                    │ Rate Limit / Routing│
                    └──────────┬──────────┘
                               │
                ┌──────────────┴──────────────┐
                │                             │
                ▼                             ▼
       ┌─────────────────┐           ┌─────────────────┐
       │ Authentication   │           │ Credit API      │
       │ Service / JWT    │           │ .NET 8          │
       └────────┬────────┘           └───────┬─────────┘
                │                            │
                │ JWT                        │
                └────────────┐      ┌────────┘
                             ▼      ▼
                       ┌───────────────┐
                       │     Redis     │
                       │    Cache      │
                       └───────┬───────┘
                               │ Cache Miss
                               ▼
                       ┌───────────────┐
                       │  PostgreSQL   │
                       │   Database    │
                       └───────────────┘

with supporting components:

- Redis for distributed caching
- Message broker for asynchronous workloads
- Centralized logging
- Metrics and alerting
- CI/CD pipeline
- Containerized deployment
- Secrets manager
- WAF/API gateway controls

### Safe deployment

- Build and test on every pull request.
- Run SAST/SCA and dependency checks in CI.
- Build an immutable artifact/container.
- Deploy to staging first.
- Run integration/smoke tests.
- Use rolling, blue-green, or canary deployment.
- Keep database migrations backward-compatible where possible.
- Provide health checks, monitoring and rollback procedures.

## Database Export Note

`skyworx_credit_backup.sql` is included as the database export captured from the development environment. The dump metadata identifies PostgreSQL 17.6. The portable `database/schema.sql` and `database/seed.sql` files are also included.

## Submission Checklist

- [x] .NET 8 project
- [x] Entity Framework Core
- [x] PostgreSQL schema
- [x] CRUD API
- [x] JWT authentication
- [x] Validation
- [x] Installment calculation
- [x] Error handling
- [x] Serilog logging
- [x] Unit tests
- [x] CI workflow
- [x] Database export
- [x] Security-sensitive local configuration removed from submission
