using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.Models;

[ApiController]
[Route("api/raw-material-lots")]
public class RawMaterialLotsController : ControllerBase
{
    private readonly AppDbContext _db;
    public RawMaterialLotsController(AppDbContext db) => _db = db;

    // GET: api/raw-material-lots
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lots = await _db.RawMaterialLots.ToListAsync();
        return Ok(lots);
    }

    // GET: api/raw-material-lots/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lot = await _db.RawMaterialLots.FindAsync(id);
        if (lot == null) return NotFound();
        return Ok(lot);
    }

    // PUT: api/raw-material-lots/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] LotStatusDto dto)
    {
        var lot = await _db.RawMaterialLots.FindAsync(id);
        if (lot == null) return NotFound();
        lot.StatusId = dto.StatusId;
        await _db.SaveChangesAsync();
        return Ok(lot);
    }
}

public class LotStatusDto
{
    public int StatusId { get; set; }
}