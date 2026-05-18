using Microsoft.EntityFrameworkCore;
using ProductionApi.Data;

namespace ProductionApi.Services
{
    public class ReportService
    {
        private readonly AppDbContext _db;
        public ReportService(AppDbContext db) => _db = db;

        public async Task<object> GetBatchReportAsync(DateTime from, DateTime to)
        {
            var batches = await _db.Batches
                .Where(b => b.StartTime >= from && b.EndTime <= to)
                .Select(b => new
                {
                    b.BatchNumber,
                    b.StartTime,
                    b.EndTime,
                    b.StatusId,
                    b.ActualQuantityKg,
                    DeviationCount = _db.BatchStepExecutions.Count(s => s.BatchId == b.Id && s.DeviationFlag)
                })
                .ToListAsync();
            return batches;
        }

        public async Task<object> GetDeviationsReportAsync(DateTime from, DateTime to)
        {
            return await _db.BatchStepExecutions
                .Where(s => s.ActualTempC != null && s.DeviationFlag)
                .Join(_db.Batches, s => s.BatchId, b => b.Id, (s, b) => new { s, b })
                .Where(x => x.b.StartTime >= from && x.b.EndTime <= to)
                .Select(x => new
                {
                    x.b.BatchNumber,
                    x.s.StepName,
                    x.s.PlannedTempC,
                    x.s.ActualTempC,
                    x.s.PlannedDurationMin,
                    x.s.ActualDurationMin,
                    x.s.PlannedPressureBar,
                    x.s.ActualPressureBar
                })
                .ToListAsync();
        }

        public async Task<object> GetRecipeUsageReportAsync()
        {
            return await _db.Batches
                .Join(_db.ProductionOrders, b => b.OrderId, o => o.Id, (b, o) => new { b, o })
                .GroupBy(x => x.o.RecipeId)
                .Select(g => new
                {
                    RecipeId = g.Key,
                    BatchCount = g.Count(),
                    TotalQuantity = g.Sum(x => x.b.ActualQuantityKg)
                })
                .ToListAsync();
        }

        public async Task<object> GetLabBlockReportAsync()
        {
            return await _db.LabTests
                .Where(t => t.Decision == "blocked")
                .Join(_db.Batches, t => t.BatchId, b => b.Id, (t, b) => new { t, b })
                .Select(x => new
                {
                    x.b.BatchNumber,
                    x.t.AnalysisDate,
                    x.t.Decision,
                    x.t.AnalystComment
                })
                .ToListAsync();
        }
    }
}