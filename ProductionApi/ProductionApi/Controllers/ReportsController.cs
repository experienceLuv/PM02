using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ReportsController(AppDbContext db) => _db = db;

    [HttpGet("batches")]
    public async Task<IActionResult> BatchReport([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var data = await _db.Batches.Where(b => b.StartTime >= from && b.EndTime <= to)
            .Select(b => new { b.Id, b.BatchNumber, b.StartTime, b.EndTime, b.StatusId, b.ActualQuantityKg })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("deviations")]
    public async Task<IActionResult> DeviationsReport([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var data = await _db.BatchStepExecutions.Where(s => s.DeviationFlag)
            .Join(_db.Batches, s => s.BatchId, b => b.Id, (s, b) => new { s, b })
            .Where(x => x.b.StartTime >= from && x.b.EndTime <= to)
            .Select(x => new { x.b.BatchNumber, x.s.StepName, x.s.PlannedTempC, x.s.ActualTempC })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("recipe-usage")]
    public async Task<IActionResult> RecipeUsageReport()
    {
        var data = await _db.Batches.Join(_db.ProductionOrders, b => b.OrderId, o => o.Id, (b, o) => new { b, o.RecipeId })
            .GroupBy(x => x.RecipeId).Select(g => new { RecipeId = g.Key, BatchCount = g.Count() })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("lab-blocks")]
    public async Task<IActionResult> LabBlockReport()
    {
        var data = await _db.LabTests.Where(t => t.Decision == "blocked")
            .Select(t => new { t.Id, t.AnalysisDate, t.Decision, t.AnalystComment })
            .ToListAsync();
        return Ok(data);
    }
}