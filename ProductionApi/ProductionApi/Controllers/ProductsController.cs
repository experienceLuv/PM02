using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.Models;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var query = _db.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search));
        var total = await query.CountAsync();
        var items = await query.OrderBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(p => new { p.Id, p.Code, p.Name, p.Type, p.Form, p.StatusId })
            .ToListAsync();
        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _db.Products.FindAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product updated)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return NotFound();
        p.Name = updated.Name; p.Type = updated.Type; p.Form = updated.Form; p.StatusId = updated.StatusId;
        await _db.SaveChangesAsync();
        return Ok(p);
    }

    [HttpPut("{id}/archive")]
    public async Task<IActionResult> Archive(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return NotFound();
        p.StatusId = 3; // archived
        await _db.SaveChangesAsync();
        return Ok(new { message = "Продукт архивирован" });
    }
}