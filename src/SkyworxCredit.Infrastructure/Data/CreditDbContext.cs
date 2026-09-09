using Microsoft.EntityFrameworkCore;
using SkyworkxCredit.Domain.Entities;

namespace SkyworxCredit.Infrastructure.Data;

public class CreditDbContext : DbContext
{
    public CreditDbContext(DbContextOptions<CreditDbContext> options)
        : base(options)
    {
    }

    public DbSet<PengajuanKredit> PengajuanKredits => Set<PengajuanKredit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PengajuanKredit>(entity =>
        {
            entity.ToTable("pengajuan_kredit");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("uuid");

            entity.Property(x => x.Plafon)
                .HasColumnName("plafon")
                .HasColumnType("numeric");

            entity.Property(x => x.Bunga)
                .HasColumnName("bunga")
                .HasColumnType("decimal(5,2)");

            entity.Property(x => x.Tenor)
                .HasColumnName("tenor")
                .HasColumnType("integer");

            entity.Property(x => x.Angsuran)
                .HasColumnName("angsuran")
                .HasColumnType("numeric");

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            entity.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            entity.HasIndex(x => x.Plafon);

            entity.HasIndex(x => x.Tenor);
        });
    }
}