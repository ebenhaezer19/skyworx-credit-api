using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyworxCredit.Application.DTOs;
using SkyworxCredit.Application.Services;
using SkyworxCredit.Application.Validators;

namespace SkyworxCredit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PengajuanKreditController : ControllerBase
{
    private readonly PengajuanKreditService _service;
    private readonly ILogger<PengajuanKreditController> _logger;

    public PengajuanKreditController(PengajuanKreditService service, ILogger<PengajuanKreditController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // POST: api/pengajuankredit
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PengajuanKreditRequest request)
    {
        var validator = new PengajuanKreditValidator();
        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors });
        }

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // GET: api/pengajuankredit/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound(new { message = $"Pengajuan kredit dengan ID {id} tidak ditemukan" });
        }
        return Ok(result);
    }

    // GET: api/pengajuankredit
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _service.GetAllAsync();
        return Ok(results);
    }

    // PUT: api/pengajuankredit/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PengajuanKreditRequest request)
    {
        var validator = new PengajuanKreditValidator();
        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors });
        }

        var result = await _service.UpdateAsync(id, request);
        if (result == null)
        {
            return NotFound(new { message = $"Pengajuan kredit dengan ID {id} tidak ditemukan" });
        }
        return Ok(result);
    }

    // DELETE: api/pengajuankredit/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Pengajuan kredit dengan ID {id} tidak ditemukan" });
        }
        return NoContent();
    }

    // POST: api/pengajuankredit/hitung-angsuran
    [HttpPost("hitung-angsuran")]
    [AllowAnonymous]
    public IActionResult HitungAngsuran([FromBody] AngsuranCalculationRequest request)
    {
        if (request.Plafon <= 0 || request.Tenor <= 0 || request.Bunga < 0 || request.Bunga > 100)
        {
            return BadRequest(new { message = "Plafon dan tenor harus > 0, bunga harus 0-100%" });
        }

        var result = _service.HitungAngsuran(request);
        return Ok(result);
    }

    // GET: api/pengajuankredit/longest-highest
    [HttpGet("longest-highest")]
    public async Task<IActionResult> GetLongestTenorAndHighestPlafon()
    {
        var result = await _service.GetLongestTenorAndHighestPlafonAsync();
        if (result == null)
        {
            return NotFound(new { message = "Belum ada data pengajuan kredit" });
        }
        return Ok(result);
    }

    // GET: api/pengajuankredit/average-bunga
    [HttpGet("average-bunga")]
    public async Task<IActionResult> GetAverageBunga()
    {
        var result = await _service.GetAverageBungaAsync();
        return Ok(new { averageBunga = result });
    }
}