using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;

[ApiController]
[Route("api/steps")]
public class StepsController : ControllerBase
{
    private readonly AppDbContext _db;
    public StepsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int batchId)
    {
        var steps = await _db.BatchStepExecutions
            .Where(s => s.BatchId == batchId)
            .OrderBy(s => s.StepOrder)
            .ToListAsync();
        return Ok(steps);
    }
}