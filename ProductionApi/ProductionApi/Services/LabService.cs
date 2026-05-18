using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;
using ProductionApi.DTOs;
using ProductionApi.Models;

namespace ProductionApi.Services
{
    public class LabService
    {
        private readonly AppDbContext _db;
        public LabService(AppDbContext db) => _db = db;

        public async Task<LabTest> CreateTestAsync(LabTestCreateDto dto)
        {
            var test = new LabTest
            {
                BatchId = dto.BatchId,
                AnalysisDate = DateTime.UtcNow,
                SampleType = dto.SampleType,
                ParameterName = dto.ParameterName,
                MeasuredValue = dto.MeasuredValue,
                StandardValue = dto.StandardValue,
                Unit = dto.Unit,
                Result = dto.Result,
                Decision = dto.Decision
            };
            _db.LabTests.Add(test);
            await _db.SaveChangesAsync();
            return test;
        }

        public async Task SetDecisionAsync(int testId, string decision, string? comment, int userId)
        {
            var test = await _db.LabTests.FindAsync(testId) ?? throw new Exception("Тест не найден");
            test.Decision = decision;
            test.AnalystComment = comment ?? test.AnalystComment;
            // Если решение "blocked", то можно автоматически заблокировать партию
            if (decision == "blocked")
            {
                var batch = await _db.Batches.FindAsync(test.BatchId);
                if (batch != null)
                {
                    batch.StatusId = 5; // предположим, статус "blocked" (создать если нет)
                }
            }
            await _db.SaveChangesAsync();
            // Запись в аудит
            _db.AuditLogs.Add(new AuditLog
            {
                EntityType = "lab_test",
                EntityId = test.Id,
                Action = "decision",
                NewValue = decision,
                ChangedBy = userId,
                ChangedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }
    }
}