using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.DTOs;
using ProductionApi.Models;

namespace ProductionApi.Services
{
    public class RecipeService
    {
        private readonly AppDbContext _db;
        public RecipeService(AppDbContext db) => _db = db;

        public async Task<RecipeVersion> CreateRecipeAsync(RecipeCreateDto dto, int userId)
        {
            var recipe = new RecipeVersion
            {
                ProductId = dto.ProductId,
                Version = dto.Version,
                StatusId = 1, // draft
                CreatedBy = userId,
                IsActive = false,
                CreatedAt = DateTime.UtcNow
            };
            _db.RecipeVersions.Add(recipe);

            foreach (var comp in dto.Components)
            {
                _db.RecipeComponents.Add(new RecipeComponent
                {
                    RecipeVersionId = recipe.Id,
                    RawMaterialId = comp.RawMaterialId,
                    Percentage = comp.Percentage,
                    LoadOrder = comp.LoadOrder,
                    ToleranceMin = comp.ToleranceMin,
                    ToleranceMax = comp.ToleranceMax
                });
            }
            await _db.SaveChangesAsync();
            return recipe;
        }

        public async Task ApproveRecipeAsync(int recipeId, int userId)
        {
            // Проверка суммы компонентов (дублируется в процедуре, но здесь для уверенности)
            var sum = await _db.RecipeComponents
                .Where(c => c.RecipeVersionId == recipeId)
                .SumAsync(c => c.Percentage);
            if (sum != 100)
                throw new InvalidOperationException("Сумма компонентов не равна 100%");

            await _db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_ApproveRecipe @recipe_id={recipeId}, @user_id={userId}");
        }
    }
}