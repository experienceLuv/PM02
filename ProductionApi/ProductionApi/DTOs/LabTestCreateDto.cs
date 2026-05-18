namespace ProductionApi.DTOs
{
    public class LabTestCreateDto
    {
        public int BatchId { get; set; }
        public string SampleType { get; set; } = "";
        public string ParameterName { get; set; } = "";
        public decimal MeasuredValue { get; set; }
        public string StandardValue { get; set; } = "";
        public string? Unit { get; set; }
        public string Result { get; set; } = "";
        public string? Decision { get; set; }
        public string? AnalystComment { get; set; }
    }
}