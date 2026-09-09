using Microsoft.EntityFrameworkCore;
using SkyworxCredit.Domain.Entities;
using SkyworxCredit.Infrastructure.Data;
using SkyworxCredit.Application.Interfaces;

namespace SkyworxCredit.Infrastructure.Repositories;

public class PengajuanKreditRepository : IPengajuanKreditRepository
{
    private readonly CreditDbContext _context;

    public PengajuanKreditRepository(CreditDbContext context)
    {
        _context = context;
    }

    public async Task<PengajuanKredit> CreateAsync(PengajuanKredit entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        
        _context.PengajuanKredits.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<PengajuanKredit?> GetByIdAsync(Guid id)
    {
        return await _context.PengajuanKredits.FindAsync(id);
    }

    public async Task<IEnumerable<PengajuanKredit>> GetAllAsync()
    {
        return await _context.PengajuanKredits
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<PengajuanKredit> UpdateAsync(PengajuanKredit entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PengajuanKredits.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.PengajuanKredits.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.PengajuanKredits.AnyAsync(x => x.Id == id);
    }

    // SQL Query untuk Soal 1
    public async Task<PengajuanKredit?> GetLongestTenorAndHighestPlafonAsync()
    {
        return await _context.PengajuanKredits
            .OrderByDescending(x => x.Tenor)
            .ThenByDescending(x => x.Plafon)
            .FirstOrDefaultAsync();
    }

    public async Task<decimal> GetAverageBungaAsync()
    {
        return await _context.PengajuanKredits
            .AverageAsync(x => x.Bunga);
    }
}