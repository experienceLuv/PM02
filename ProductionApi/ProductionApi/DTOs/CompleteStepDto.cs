namespace ProductionApi.DTOs
{
    public class CompleteStepDto
    {
        public decimal ActualTemp { get; set; }
        public int ActualDuration { get; set; }
        public decimal ActualPressure { get; set; }
        public string Comment { get; set; } = "";
    }
}