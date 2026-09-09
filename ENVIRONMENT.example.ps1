# PowerShell example - set these locally before running the API.
# Do not commit real secrets to source control.

$env:ConnectionStrings__DefaultConnection = "Host=localhost;Database=skyworx_credit;Username=postgres;Password=<YOUR_POSTGRES_PASSWORD>"
$env:Jwt__Key = "<GENERATE_A_LONG_RANDOM_SECRET>"
$env:Auth__Username = ""
$env:Auth__Password = ""

dotnet restore
dotnet build
dotnet test
dotnet run --project src/SkyworxCredit.Api/SkyworxCredit.Api.csproj
