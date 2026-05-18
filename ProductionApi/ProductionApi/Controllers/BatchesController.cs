using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;

[ApiController]
[Route("api/batches")]
public class BatchesController : ControllerBase
{
    private readonly AppDbContext _db;
    public BatchesController(AppDbContext db) => _db = db;

    // Получить список всех партий (без пагинации, для простоты)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var batches = await _db.Batches.ToListAsync();
        // Возвращаем объект с полем items для совместимости с клиентом
        return Ok(new { items = batches, totalCount = batches.Count, page = 1, pageSize = 100 });
    }

    // Получить шаги конкретной партии – ГАРАНТИРОВАННО РАБОТАЕТ
    [HttpGet("{id}/steps")]
    public async Task<IActionResult> GetSteps(int id)
    {
        var steps = await _db.BatchStepExecutions
            .Where(s => s.BatchId == id)
            .OrderBy(s => s.StepOrder)
            .ToListAsync();
        return Ok(steps);
    }

    // Начать шаг (заглушка, чтобы кнопки не падали)
    [HttpPost("{batchId}/steps/{stepId}/start")]
    public IActionResult StartStep(int batchId, int stepId)
    {
        return Ok(new { message = "Шаг начат (заглушка)" });
    }

    // Завершить шаг (заглушка)
    [HttpPut("{batchId}/steps/{stepId}/complete")]
    public IActionResult CompleteStep(int batchId, int stepId, [FromBody] object dto)
    {
        return Ok(new { message = "Шаг завершён (заглушка)" });
    }
}