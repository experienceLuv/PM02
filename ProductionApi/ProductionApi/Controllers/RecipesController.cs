using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.Models;

[ApiController]
[Route("api/recipes")]
public class RecipesController : ControllerBase
{
    private readonly AppDbContext _db;
    public RecipesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _db.RecipeVersions.AsQueryable();
        var total = await query.CountAsync();
        var items = await query.OrderBy(r => r.Id).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(r => new {
                r.Id,
                r.ProductId,
                r.Version,
                r.StatusId,
                r.IsActive,
                r.CreatedAt,
                r.ApprovedAt,
                Components = r.RecipeComponents.Select(c => new { c.Id, c.RawMaterialId, c.Percentage, c.LoadOrder, c.ToleranceMin, c.ToleranceMax })
            })
            .ToListAsync();
        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var sum = await _db.RecipeComponents.Where(c => c.RecipeVersionId == id).SumAsync(c => c.Percentage);
        if (sum != 100) return BadRequest(new { message = "Сумма компонентов не равна 100%" });

        var activeStatusId = await _db.Statuses.Where(s => s.EntityType == "recipe" && s.Name == "active").Select(s => s.Id).FirstAsync();
        var recipe = await _db.RecipeVersions.FindAsync(id);
        if (recipe == null) return NotFound();

        var prev = await _db.RecipeVersions.Where(r => r.ProductId == recipe.ProductId && r.IsActive && r.StatusId == activeStatusId).FirstOrDefaultAsync();
        if (prev != null) prev.IsActive = false;

        recipe.StatusId = activeStatusId; recipe.IsActive = true; recipe.ApprovedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Рецептура утверждена" });
    }

    [HttpPut("{id}/archive")]
    public async Task<IActionResult> Archive(int id)
    {
        var r = await _db.RecipeVersions.FindAsync(id);
        if (r == null) return NotFound();
        r.StatusId = 3;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Рецептура архивирована" });
    }
}