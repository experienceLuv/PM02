namespace ProductionApi.DTOs
{
    public class LabTestResultsDto
    {
        public decimal MeasuredValue { get; set; }
        public string Result { get; set; } = "";
        public string? Comment { get; set; }
    }

}