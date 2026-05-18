namespace ProductionApi.DTOs
{
    public class TechnologicalMapCreateDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public List<TechnologicalMapStepDto> Steps { get; set; } = new();
    }

    public class TechnologicalMapStepDto
    {
        public int StepOrder { get; set; }
        public string StepName { get; set; } = "";
        public string StepType { get; set; } = "common";
        public decimal? PlannedTempC { get; set; }
        public int? PlannedDurationMin { get; set; }
        public decimal? PlannedPressureBar { get; set; }
        public bool IsMandatory { get; set; } = true;
        public string? Instruction { get; set; }
    }
}