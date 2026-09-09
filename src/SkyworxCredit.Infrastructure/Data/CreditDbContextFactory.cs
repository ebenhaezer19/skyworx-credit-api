using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SkyworxCredit.Infrastructure.Data;

public class CreditDbContextFactory : IDesignTimeDbContextFactory<CreditDbContext>
{
    public CreditDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CreditDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=skyworx_credit;Username=postgres;Password=Postgres@123");
        return new CreditDbContext(optionsBuilder.Options);
    }
}