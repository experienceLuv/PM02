using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.DTOs;
using ProductionApi.Models;

namespace ProductionApi.Services
{
    public class MapService
    {
        private readonly AppDbContext _db;
        public MapService(AppDbContext db) => _db = db;

        public async Task<TechnologicalMap> CreateMapAsync(TechnologicalMapCreateDto dto, int userId)
        {
            var map = new TechnologicalMap
            {
                ProductId = dto.ProductId,
                Version = 1, // автоматически или передавать
                Name = dto.Name,
                StatusId = await _db.Statuses.Where(s => s.EntityType == "card" && s.Name == "draft").Select(s => s.Id).FirstAsync(),
                CreatedBy = userId,
                IsActive = false,
                CreatedAt = DateTime.UtcNow
            };
            _db.TechnologicalMaps.Add(map);
            foreach (var stepDto in dto.Steps)
            {
                _db.TechnologicalMapSteps.Add(new TechnologicalMapStep
                {
                    MapId = map.Id,
                    StepOrder = stepDto.StepOrder,
                    StepName = stepDto.StepName,
                    StepType = stepDto.StepType,
                    PlannedTempC = stepDto.PlannedTempC,
                    PlannedDurationMin = stepDto.PlannedDurationMin,
                    PlannedPressureBar = stepDto.PlannedPressureBar,
                    IsMandatory = stepDto.IsMandatory,
                    Instruction = stepDto.Instruction
                });
            }
            await _db.SaveChangesAsync();
            return map;
        }

        public async Task ApproveMapAsync(int mapId, int userId)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_ApproveTechnologicalMap @map_id={mapId}, @user_id={userId}");
        }
    }
}