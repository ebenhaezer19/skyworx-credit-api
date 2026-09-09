# Skyworx Credit Application API

Backend technical test implementation for Credit Application Management using .NET 8, Entity Framework Core, and PostgreSQL.

---

## 🚀 Tech Stack

| Technology | Version | Description |
|---|---|---|
| **.NET** | 8.0 | ASP.NET Core Web API |
| **Entity Framework Core** | 8.0.11 | ORM untuk database |
| **PostgreSQL** | 16.3 | Database |
| **JWT Authentication** | 7.6.3 | Bearer token authentication |
| **FluentValidation** | 11.11.0 | Input validation |
| **Serilog** | 8.0.3 | Request/Response logging |
| **xUnit** | Latest | Unit testing |
| **Swagger** | 6.6.2 | API Documentation |

---

## ✨ Features

### Authentication & Authorization
- ✅ JWT Bearer Token Authentication
- ✅ Secure API endpoints (only authenticated users can access)
- ✅ Login endpoint with hardcoded credentials for demo

### Credit Application Management (CRUD)
- ✅ **Create** — Submit new credit application
- ✅ **Read** — Get all applications or by ID
- ✅ **Update** — Modify existing application
- ✅ **Delete** — Remove application

### Validation
- ✅ Plafon > 0
- ✅ Bunga between 0–100%
- ✅ Tenor > 0

### Installment Calculation
- ✅ Calculate monthly installment using annuity formula
- ✅ Return total payment and total interest

### Additional Features
- ✅ **Logging** — Serilog with file output
- ✅ **Error Handling** — Global exception middleware
- ✅ **Indexing** — Database indexes on `plafon` and `tenor`
- ✅ **Unit Testing** — 17 test cases covering CRUD and calculations
- ✅ **Swagger** — OpenAPI 3.0 documentation

---

## 🏗️ Architecture (Clean Architecture)

| Layer | Project | Components | Depends On |
|---|---|---|---|
| **API** | `SkyworxCredit.Api` | Controllers, Middleware, Swagger | App + Infrastructure |
| **Application** | `SkyworxCredit.Application` | DTOs, Services, Interfaces, Validators | Domain |
| **Infrastructure** | `SkyworxCredit.Infrastructure` | DbContext, Repositories, Migrations | App + Domain |
| **Domain** | `SkyworxCredit.Domain` | Entities (PengajuanKredit) | (None) |

**Dependency Direction:** API → Application → Infrastructure → Domain
**Reference Rule:** Domain has no dependencies on other projects.

### Project References

| Project | References |
|---|---|
| `SkyworxCredit.Api` | → `Application`, `Infrastructure` |
| `SkyworxCredit.Application` | → `Domain` |
| `SkyworxCredit.Infrastructure` | → `Application`, `Domain` |
| `SkyworxCredit.Domain` | → (None) |

### Layered Architecture (Clean Architecture)
1. **Domain** — Entities (no dependencies)
2. **Application** — Business logic, DTOs, Services, Interfaces
3. **Infrastructure** — Data access, Repositories, EF Core
4. **API** — Controllers, Middleware, Configuration

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 16.3](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Installation

**1. Clone the repository**
```bash
git clone <repository-url>
cd skyworx-credit-api
```

**2. Update Connection String**

Edit `src/SkyworxCredit.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=skyworx_credit;Username=postgres;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "Key": "your-secret-key-min-16-characters",
    "Issuer": "SkyworxCredit",
    "Audience": "SkyworxCreditApi"
  }
}
```

**3. Create Database**
```bash
# Connect to PostgreSQL
psql -U postgres -h localhost

# Create database
CREATE DATABASE skyworx_credit;

# Exit psql
\q
```

**4. Apply Migrations**
```bash
dotnet ef database update --project src/SkyworxCredit.Infrastructure --startup-project src/SkyworxCredit.Api
```

**5. Run the API**
```bash
dotnet run --project src/SkyworxCredit.Api/SkyworxCredit.Api.csproj --launch-profile http
```

**6. Access Swagger**
```
http://localhost:5190/swagger
```

---

## 📚 API Documentation

### Authentication

**Login**
```http
POST /api/Auth/login
```

Request Body:
```json
{
  "username": "admin",
  "password": "password123"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Use Token:** Add `Bearer {token}` to the Authorization header.

### Endpoints

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/Auth/login` | Login | ❌ |
| POST | `/api/PengajuanKredit` | Create credit application | ✅ |
| GET | `/api/PengajuanKredit` | Get all applications | ✅ |
| GET | `/api/PengajuanKredit/{id}` | Get application by ID | ✅ |
| PUT | `/api/PengajuanKredit/{id}` | Update application | ✅ |
| DELETE | `/api/PengajuanKredit/{id}` | Delete application | ✅ |
| POST | `/api/PengajuanKredit/hitung-angsuran` | Calculate installment | ❌ |
| GET | `/api/PengajuanKredit/longest-highest` | Longest tenor & highest plafon | ✅ |
| GET | `/api/PengajuanKredit/average-bunga` | Average interest rate | ✅ |

### Example Requests

**Create Credit Application**
```http
POST /api/PengajuanKredit
Authorization: Bearer {token}
Content-Type: application/json

{
  "plafon": 100000000,
  "bunga": 12,
  "tenor": 60
}
```

Response (201 Created):
```json
{
  "id": "ce984076-06b3-4dff-a6c1-63187c1bb533",
  "plafon": 100000000,
  "bunga": 12,
  "tenor": 60,
  "angsuran": 2224444.77,
  "createdAt": "2026-09-09T11:04:40.1371725Z",
  "updatedAt": "2026-09-09T11:04:40.1372668Z"
}
```

**Calculate Installment**
```http
POST /api/PengajuanKredit/hitung-angsuran
Content-Type: application/json

{
  "plafon": 100000000,
  "bunga": 12,
  "tenor": 60
}
```

Response:
```json
{
  "plafon": 100000000,
  "bunga": 12,
  "tenor": 60,
  "angsuranPerBulan": 2224444.77,
  "totalPembayaran": 133466686.20,
  "totalBunga": 33466686.20
}
```

**Longest Tenor & Highest Plafon**
```http
GET /api/PengajuanKredit/longest-highest
Authorization: Bearer {token}
```

**Average Interest Rate**
```http
GET /api/PengajuanKredit/average-bunga
Authorization: Bearer {token}
```

---

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Test Coverage

**Total Tests:** 17 — All Passed ✅

| Test Class | Tests | Status |
|---|---|---|
| AngsuranCalculatorTests | 3 | ✅ Passed |
| PengajuanKreditServiceTests | 7 | ✅ Passed |
| PengajuanKreditValidatorTests | 5 | ✅ Passed |
| Others | 2 | ✅ Passed |

### Test Scenarios
- **Angsuran Calculation** — Valid data, zero interest, large values
- **CRUD Operations** — Create, Read (by ID & all), Update, Delete
- **Validation** — Plafon, Bunga, Tenor validation rules

---

## 📊 SQL Queries (Soal 1)

**Longest Tenor & Highest Plafon**
```sql
SELECT * FROM pengajuan_kredit
ORDER BY tenor DESC, plafon DESC
LIMIT 1;
```

**Average Interest Rate**
```sql
SELECT AVG(bunga) FROM pengajuan_kredit;
```

---

## 📈 Performance & Scalability

### Database Optimization
- ✅ Index on `plafon` column
- ✅ Index on `tenor` column
- ✅ Primary key on `id` (UUID)

### Caching Strategy (Redis)
- Cache frequently accessed data (Get All, Get By ID)
- TTL: 5 minutes for read operations
- Invalidate cache on Create, Update, Delete

### Load Balancing
- Horizontal scaling with multiple instances
- API Gateway (YARP/Ocelot) for routing
- Round-robin load balancing

### Microservices Architecture
- **API Gateway** — Route requests to appropriate services
- **Authentication Service** — Handle JWT validation
- **Credit Service** — Manage credit applications
- Database per service (microservices pattern)
- Message Broker (RabbitMQ/Kafka) for async communication

---

## 🏗️ System Design (Enterprise Scale)

```
┌─────────────────────────────────────────────────────────────────┐
│                         Client Apps                             │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                      API Gateway                                │
│          (YARP / Ocelot - Load Balancing)                       │
└─────────────────────────────────────────────────────────────────┘
                │              │              │
                ▼              ▼              ▼
┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐
│  Auth Service    │ │  Credit Service  │ │  User Service    │
│  (JWT)           │ │  (CRUD)          │ │  (Profile)       │
└──────────────────┘ └──────────────────┘ └──────────────────┘
                │              │              │
                └──────────────┼──────────────┘
                               ▼
                    ┌──────────────────────┐
                    │   PostgreSQL         │
                    │   (Master-Slave)     │
                    └──────────────────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │   Redis Cache        │
                    │   (Caching Layer)    │
                    └──────────────────────┘
```

### CI/CD Pipeline
1. **Build** → GitHub Actions / GitLab CI
2. **Test** → Run Unit Tests
3. **Lint** → Code Quality Check
4. **Dockerize** → Build Container Image
5. **Push** → Push to Container Registry
6. **Deploy** → Kubernetes / Docker Swarm
7. **Monitor** → Health Checks & Alerts

### Deployment Strategy
- **Blue-Green Deployment** for zero downtime
- **Canary Releases** for gradual rollout
- **Rollback** capability on failure

---

## 👨‍💻 Code Review & Leadership

### Code Review Process
1. **Pull Request** → Create PR with description
2. **Automated Checks** → CI runs tests & linters
3. **Peer Review** → At least 2 approvals
4. **Review Criteria:**
   - Code readability & maintainability
   - Test coverage
   - Performance implications
   - Security concerns
5. **Merge** → Squash merge to main

### Mentoring Junior Developers
- **Pair Programming** — Hands-on guidance
- **Code Walkthroughs** — Explain design decisions
- **Documentation** — Write clear documentation
- **Knowledge Sharing** — Weekly tech talks
- **Gradual Responsibility** — Start with small tasks, scale up

---

## 🔧 Troubleshooting

### 1. Database Connection Failed
```bash
# Check PostgreSQL service
Get-Service -Name *postgres*

# Start PostgreSQL
Start-Service -Name postgresql-x64-17
```

### 2. Migration Failed
```bash
# Remove and recreate migrations
dotnet ef migrations remove --project src/SkyworxCredit.Infrastructure
dotnet ef migrations add InitialCreate --project src/SkyworxCredit.Infrastructure
dotnet ef database update --project src/SkyworxCredit.Infrastructure
```

### 3. Port Already in Use
```bash
# Find process using port 5190
netstat -ano | findstr :5190

# Kill process (replace PID)
taskkill /PID <PID> /F
```

---

## 📝 License

This project is for technical test purposes only. Property of PT Skyworx Indonesia.

---

## 👤 Author

Eben

---

## 🎯 Project Status

| Feature | Status |
|---|---|
| Database & Query Optimization | ✅ Completed |
| API CRUD with Authentication | ✅ Completed |
| Installment Calculation API | ✅ Completed |
| Unit Testing | ✅ Completed (17 tests) |
| Error Handling & Logging | ✅ Completed |
| Performance & Scalability | ✅ Documented |
| System Design & Leadership | ✅ Documented |
