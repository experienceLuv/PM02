using System;
using System.Collections.Generic;

namespace OperatorClient.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
    }

    public class Batch
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; }
        public int OrderId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int StatusId { get; set; }
        public decimal? ActualQuantityKg { get; set; }
    }

    public class BatchStepExecution
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; }
        public decimal? PlannedTempC { get; set; }
        public decimal? ActualTempC { get; set; }
        public int? PlannedDurationMin { get; set; }
        public int? ActualDurationMin { get; set; }
        public decimal? PlannedPressureBar { get; set; }
        public decimal? ActualPressureBar { get; set; }
        public bool DeviationFlag { get; set; }
        public string OperatorComment { get; set; }
    }

    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string Action { get; set; }
        public DateTime ChangedAt { get; set; }
        public string NewValue { get; set; }
    }

    public class CompleteStepDto
    {
        public decimal ActualTemp { get; set; }
        public int ActualDuration { get; set; }
        public decimal ActualPressure { get; set; }
        public string Comment { get; set; } = "";
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}