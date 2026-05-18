using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.Models;

namespace ProductionApi.Services
{
    public class ProductionService
    {
        private readonly AppDbContext _db;
        public ProductionService(AppDbContext db) => _db = db;

        // Создание партии из заказа
        public async Task<Batch> CreateBatchFromOrderAsync(int orderId, int userId)
        {
            var order = await _db.ProductionOrders.FindAsync(orderId)
                        ?? throw new Exception("Заказ не найден");

            var recipe = await _db.RecipeVersions.FindAsync(order.RecipeId)
                         ?? throw new Exception("Рецепт не найден");

            if (recipe.StatusId != 2)
                throw new Exception("Рецептура не утверждена");

            var map = await _db.TechnologicalMaps
                .FirstOrDefaultAsync(m => m.ProductId == recipe.ProductId && m.IsActive)
                ?? throw new Exception("Нет активной технологической карты");

            var steps = await _db.TechnologicalMapSteps
                .Where(s => s.MapId == map.Id)
                .OrderBy(s => s.StepOrder)
                .ToListAsync();

            var batch = new Batch
            {
                BatchNumber = $"B-{DateTime.UtcNow:yyMMdd}-{order.OrderNumber}",
                OrderId = order.Id,
                StatusId = 9,
                StartTime = null,
                EndTime = null,
                ActualQuantityKg = 0
            };
            _db.Batches.Add(batch);

            foreach (var step in steps)
            {
                _db.BatchStepExecutions.Add(new BatchStepExecution
                {
                    BatchId = batch.Id,
                    StepOrder = step.StepOrder,
                    StepName = step.StepName,
                    PlannedTempC = step.PlannedTempC,
                    PlannedDurationMin = step.PlannedDurationMin,
                    PlannedPressureBar = step.PlannedPressureBar,
                    DeviationFlag = false
                });
            }

            order.StatusId = 6; // in_progress
            await _db.SaveChangesAsync();
            return batch;
        }

        // Запуск шага партии
        public async Task StartStepAsync(int stepId, int userId)
        {
            var step = await _db.BatchStepExecutions.FindAsync(stepId)
                       ?? throw new Exception("Шаг не найден");
            var batch = await _db.Batches.FindAsync(step.BatchId);
            if (batch == null) throw new Exception("Партия не найдена");

            // Переводим статус партии в "running", если ещё нет
            if (batch.StatusId != 10) // 10 = running
            {
                batch.StatusId = 10;
                batch.StartTime ??= DateTime.UtcNow;
            }

            // Здесь можно добавить запись в аудит (опционально)
            _db.AuditLogs.Add(new AuditLog
            {
                EntityType = "step",
                EntityId = step.Id,
                Action = "start",
                NewValue = $"Шаг {step.StepName} начат пользователем {userId}",
                ChangedBy = userId,
                ChangedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }

        // Завершение шага с фиксацией фактических параметров
        public async Task CompleteStepAsync(int stepId, decimal actualTemp, int actualDuration, decimal actualPressure, string comment, int userId)
        {
            var step = await _db.BatchStepExecutions.FindAsync(stepId)
                       ?? throw new Exception("Шаг не найден");
            step.ActualTempC = actualTemp;
            step.ActualDurationMin = actualDuration;
            step.ActualPressureBar = actualPressure;
            step.OperatorComment = comment;

            // Вычисляем отклонение (можно использовать ту же логику, что в процедуре)
            step.DeviationFlag = Math.Abs(actualTemp - (step.PlannedTempC ?? 0)) > 2 ||
                                 Math.Abs(actualDuration - (step.PlannedDurationMin ?? 0)) > 5 ||
                                 Math.Abs(actualPressure - (step.PlannedPressureBar ?? 0)) > 0.5m;

            // Проверяем, все ли обязательные шаги завершены
            var batchSteps = await _db.BatchStepExecutions
                .Where(s => s.BatchId == step.BatchId)
                .ToListAsync();
            var allCompleted = batchSteps.All(s => s.ActualTempC != null || s.StepOrder == step.StepOrder);
            if (allCompleted)
            {
                var batch = await _db.Batches.FindAsync(step.BatchId);
                if (batch != null)
                {
                    batch.StatusId = 11; // completed
                    batch.EndTime = DateTime.UtcNow;
                }
            }

            _db.AuditLogs.Add(new AuditLog
            {
                EntityType = "step",
                EntityId = step.Id,
                Action = "complete",
                NewValue = $"Шаг {step.StepName} завершён, отклонение: {step.DeviationFlag}",
                ChangedBy = userId,
                ChangedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }
    }
}