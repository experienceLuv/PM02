using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.DTOs;
using ProductionApi.Models;

[ApiController]
[Route("api/lab-tests")]
public class LabTestsController : ControllerBase
{
    private readonly AppDbContext _db;
    public LabTestsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? rawMaterialLotId)
    {
        var query = _db.LabTests.AsQueryable();
        if (rawMaterialLotId.HasValue)
            query = query.Where(t => t.RawMaterialLotId == rawMaterialLotId.Value);

        var items = await query
            .OrderBy(t => t.Id)
            .Select(t => new
            {
                t.Id,
                t.BatchId,
                t.RawMaterialLotId,
                t.AnalysisDate,
                t.SampleType,
                t.ParameterName,
                t.MeasuredValue,
                t.StandardValue,
                t.Unit,
                t.Result,
                t.Decision,
                t.AnalystComment
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LabTest test)
    {
        test.AnalysisDate = DateTime.UtcNow;
        _db.LabTests.Add(test);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = test.Id }, test);
    }

    [HttpPut("{id}/results")]
    public async Task<IActionResult> SetResults(int id, [FromBody] LabTestResultsDto dto)
    {
        var test = await _db.LabTests.FindAsync(id);
        if (test == null) return NotFound();
        test.MeasuredValue = dto.MeasuredValue;
        test.Result = dto.Result;
        test.AnalystComment = dto.Comment ?? test.AnalystComment;
        await _db.SaveChangesAsync();
        return Ok(test);
    }

    [HttpPut("{id}/decision")]
    public async Task<IActionResult> SetDecision(int id, [FromBody] LabDecisionDto dto)
    {
        var test = await _db.LabTests.FindAsync(id);
        if (test == null) return NotFound();
        test.Decision = dto.Decision;
        test.AnalystComment = dto.Comment ?? test.AnalystComment;
        await _db.SaveChangesAsync();
        return Ok(test);
    }
}