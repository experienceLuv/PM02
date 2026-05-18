using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionApi.Data;
using ProductionApi.DTOs;
using ProductionApi.Models;
using ProductionApi.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/orders")]
[Authorize]
public class ProductionOrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ProductionService _productionService;
    public ProductionOrdersController(AppDbContext db, ProductionService productionService) { _db = db; _productionService = productionService; }

    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {
        var query = _db.ProductionOrders;
        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(new PaginatedResponse<ProductionOrder> { Items = items, TotalCount = total, Page = page, PageSize = pageSize });
    }

    [HttpPost]
    [Authorize(Policy = "Technologist")]
    public async Task<IActionResult> Create(ProductionOrder order)
    {
        _db.ProductionOrders.Add(order);
        await _db.SaveChangesAsync();
        return Ok(order);
    }

    [HttpPost("{orderId}/start-batch")]
    [Authorize(Policy = "Technologist")]
    public async Task<IActionResult> StartBatch(int orderId)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var batch = await _productionService.CreateBatchFromOrderAsync(orderId, userId);
        return Ok(batch);
    }
}