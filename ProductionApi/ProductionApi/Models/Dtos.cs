namespace ProductionApi.Models
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Version { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public List<ComponentDto> Components { get; set; } = new();
    }

    public class ComponentDto
    {
        public int Id { get; set; }
        public int RawMaterialId { get; set; }
        public decimal Percentage { get; set; }
        public int? LoadOrder { get; set; }
        public decimal? ToleranceMin { get; set; }
        public decimal? ToleranceMax { get; set; }
    }

    public class TechnologicalMapDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; } = "";
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TechnologicalMapStepDto> Steps { get; set; } = new();
    }

    public class TechnologicalMapStepDto
    {
        public int Id { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; } = "";
        public string StepType { get; set; } = "";
        public decimal? PlannedTempC { get; set; }
        public int? PlannedDurationMin { get; set; }
        public decimal? PlannedPressureBar { get; set; }
        public bool IsMandatory { get; set; }
        public string? Instruction { get; set; }
    }
}