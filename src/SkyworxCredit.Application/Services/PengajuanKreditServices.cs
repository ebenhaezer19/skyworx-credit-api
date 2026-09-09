using SkyworxCredit.Application.DTOs;
using SkyworxCredit.Domain.Entities;
using SkyworxCredit.Application.Interfaces;

namespace SkyworxCredit.Application.Services;

public class PengajuanKreditService
{
    private readonly IPengajuanKreditRepository _repository;

    public PengajuanKreditService(IPengajuanKreditRepository repository)
    {
        _repository = repository;
    }

    public async Task<PengajuanKreditResponse> CreateAsync(PengajuanKreditRequest request)
    {
        var angsuran = HitungAngsuran(request.Plafon, request.Bunga, request.Tenor);
        
        var entity = new PengajuanKredit
        {
            Plafon = request.Plafon,
            Bunga = request.Bunga,
            Tenor = request.Tenor,
            Angsuran = angsuran
        };

        var result = await _repository.CreateAsync(entity);
        return MapToResponse(result);
    }

    public async Task<PengajuanKreditResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<PengajuanKreditResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(MapToResponse);
    }

    public async Task<PengajuanKreditResponse?> UpdateAsync(Guid id, PengajuanKreditRequest request)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;

        entity.Plafon = request.Plafon;
        entity.Bunga = request.Bunga;
        entity.Tenor = request.Tenor;
        entity.Angsuran = HitungAngsuran(request.Plafon, request.Bunga, request.Tenor);

        var result = await _repository.UpdateAsync(entity);
        return MapToResponse(result);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (!await _repository.ExistsAsync(id)) return false;
        await _repository.DeleteAsync(id);
        return true;
    }

    // Untuk Soal 3 - Perhitungan Angsuran
    public AngsuranCalculationResponse HitungAngsuran(AngsuranCalculationRequest request)
    {
        var angsuran = HitungAngsuran(request.Plafon, request.Bunga, request.Tenor);
        var totalPembayaran = angsuran * request.Tenor;
        var totalBunga = totalPembayaran - request.Plafon;

        return new AngsuranCalculationResponse
        {
            Plafon = request.Plafon,
            Bunga = request.Bunga,
            Tenor = request.Tenor,
            AngsuranPerBulan = angsuran,
            TotalPembayaran = totalPembayaran,
            TotalBunga = totalBunga
        };
    }

    private decimal HitungAngsuran(decimal plafon, decimal bunga, int tenor)
    {
        decimal bungaBulanan = bunga / 100 / 12;
        
        if (bungaBulanan == 0)
        {
            return Math.Round(plafon / tenor, 2);
        }
        
        double r = (double)bungaBulanan;
        int n = tenor;
        double faktor = Math.Pow(1 + r, n);
        double angsuran = (double)plafon * (r * faktor) / (faktor - 1);
        
        return (decimal)Math.Round(angsuran, 2);
    }

    // Untuk Soal 1 - SQL Query
    public async Task<PengajuanKreditResponse?> GetLongestTenorAndHighestPlafonAsync()
    {
        var result = await _repository.GetLongestTenorAndHighestPlafonAsync();
        return result == null ? null : MapToResponse(result);
    }

    public async Task<decimal> GetAverageBungaAsync()
    {
        return await _repository.GetAverageBungaAsync();
    }

    private static PengajuanKreditResponse MapToResponse(PengajuanKredit entity)
    {
        return new PengajuanKreditResponse
        {
            Id = entity.Id,
            Plafon = entity.Plafon,
            Bunga = entity.Bunga,
            Tenor = entity.Tenor,
            Angsuran = entity.Angsuran,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}