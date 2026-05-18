using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;

[ApiController]
[Route("api/batch-steps")]
public class BatchStepsController : ControllerBase
{
    private readonly AppDbContext _db;
    public BatchStepsController(AppDbContext db) => _db = db;

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