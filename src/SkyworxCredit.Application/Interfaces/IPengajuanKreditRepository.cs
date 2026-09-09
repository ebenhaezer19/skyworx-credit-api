using SkyworxCredit.Domain.Entities;

namespace SkyworxCredit.Application.Interfaces;

public interface IPengajuanKreditRepository
{
    Task<PengajuanKredit> CreateAsync(PengajuanKredit entity);
    Task<PengajuanKredit?> GetByIdAsync(Guid id);
    Task<IEnumerable<PengajuanKredit>> GetAllAsync();
    Task<PengajuanKredit> UpdateAsync(PengajuanKredit entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    
    // Untuk Soal 1 - SQL Query
    Task<PengajuanKredit?> GetLongestTenorAndHighestPlafonAsync();
    Task<decimal> GetAverageBungaAsync();
}