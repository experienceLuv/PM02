using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _db.ProductionOrders
            .Select(o => new { o.Id, o.OrderNumber, o.RecipeId, o.PlannedQuantityKg, o.StatusId, o.PlannedStartDate })
            .ToListAsync();
        return Ok(new { items = orders, totalCount = orders.Count });
    }
}