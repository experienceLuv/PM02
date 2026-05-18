using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.Models;

[ApiController]
[Route("api/maps")]
public class TechnologicalMapsController : ControllerBase
{
    private readonly AppDbContext _db;
    public TechnologicalMapsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _db.TechnologicalMaps.AsQueryable();
        var total = await query.CountAsync();
        var items = await query.OrderBy(m => m.Id).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(m => new {
                m.Id,
                m.ProductId,
                m.Version,
                m.Name,
                m.StatusId,
                m.IsActive,
                m.CreatedAt,
                Steps = m.Steps.OrderBy(s => s.StepOrder).Select(s => new { s.Id, s.StepOrder, s.StepName, s.StepType, s.PlannedTempC, s.PlannedDurationMin, s.PlannedPressureBar, s.IsMandatory, s.Instruction })
            })
            .ToListAsync();
        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var activeStatusId = await _db.Statuses.Where(s => s.EntityType == "card" && s.Name == "active").Select(s => s.Id).FirstAsync();
        var map = await _db.TechnologicalMaps.FindAsync(id);
        if (map == null) return NotFound();

        var prev = await _db.TechnologicalMaps.Where(m => m.ProductId == map.ProductId && m.IsActive && m.StatusId == activeStatusId).FirstOrDefaultAsync();
        if (prev != null) prev.IsActive = false;

        map.StatusId = activeStatusId; map.IsActive = true;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Техкарта утверждена" });
    }

    [HttpPut("{id}/archive")]
    public async Task<IActionResult> Archive(int id)
    {
        var m = await _db.TechnologicalMaps.FindAsync(id);
        if (m == null) return NotFound();
        m.StatusId = 3;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Техкарта архивирована" });
    }
}