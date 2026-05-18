using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionApi.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; } = "";

        [Column("password_hash")]
        public string PasswordHash { get; set; } = "";

        [Column("full_name")]
        public string FullName { get; set; } = "";

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class Status
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = "";
        public string? Type { get; set; }
        public string? Form { get; set; }
        public int? StatusId { get; set; }
    }

    public class RawMaterial
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? MaterialType { get; set; }
        public string? Unit { get; set; }
    }

    public class RecipeVersion
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Version { get; set; }
        public int StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public ICollection<RecipeComponent> RecipeComponents { get; set; } = new List<RecipeComponent>();
    }

    public class RecipeComponent
    {
        public int Id { get; set; }
        public int RecipeVersionId { get; set; }
        public int RawMaterialId { get; set; }
        public decimal Percentage { get; set; }
        public int? LoadOrder { get; set; }
        public decimal? ToleranceMin { get; set; }
        public decimal? ToleranceMax { get; set; }
    }

    public class ProductionOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = "";
        public int RecipeId { get; set; }
        public RecipeVersion? Recipe { get; set; }
        public decimal PlannedQuantityKg { get; set; }
        public int StatusId { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Batch
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; } = "";
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
        public string StepName { get; set; } = "";
        public decimal? PlannedTempC { get; set; }
        public decimal? ActualTempC { get; set; }
        public int? PlannedDurationMin { get; set; }
        public int? ActualDurationMin { get; set; }
        public decimal? PlannedPressureBar { get; set; }
        public decimal? ActualPressureBar { get; set; }
        public bool DeviationFlag { get; set; }
        public string? OperatorComment { get; set; }
    }

    public class LabTest
    {
        public int Id { get; set; }
        public int? BatchId { get; set; }               // допускает NULL (для сырья)
        public int? RawMaterialLotId { get; set; }      // <-- обязательно!
        public DateTime AnalysisDate { get; set; }
        public string SampleType { get; set; } = "";
        public string ParameterName { get; set; } = "";
        public decimal MeasuredValue { get; set; }
        public string StandardValue { get; set; } = "";
        public string? Unit { get; set; }
        public string Result { get; set; } = "";
        public string? Decision { get; set; }
        public string? AnalystComment { get; set; }
    }

    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = "";
        public int EntityId { get; set; }
        public string Action { get; set; } = "";
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }

    public class TechnologicalMap
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; } = "";
        public int StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<TechnologicalMapStep> Steps { get; set; } = new List<TechnologicalMapStep>();
    }

    public class TechnologicalMapStep
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; } = "";
        public string StepType { get; set; } = "common";
        public decimal? PlannedTempC { get; set; }
        public int? PlannedDurationMin { get; set; }
        public decimal? PlannedPressureBar { get; set; }
        public bool IsMandatory { get; set; } = true;
        public string? Instruction { get; set; }
        public decimal? ToleranceTemp { get; set; }
        public int? ToleranceDuration { get; set; }
        public decimal? TolerancePressure { get; set; }
    }

    public class RawMaterialLot
    {
        public int Id { get; set; }
        public int RawMaterialId { get; set; }
        public string LotNumber { get; set; } = "";
        public string? Supplier { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "кг";
        public int StatusId { get; set; }
    }

    public class Equipment
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Type { get; set; }
        public string? Line { get; set; }
        public string? Location { get; set; }
        public int? StatusId { get; set; }
    }

}