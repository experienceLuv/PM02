namespace ProductionApi.DTOs
{
    public class RecipeCreateDto
    {
        public int ProductId { get; set; }
        public int Version { get; set; }
        public List<RecipeComponentDto> Components { get; set; } = new();
    }

    public class RecipeComponentDto
    {
        public int RawMaterialId { get; set; }
        public decimal Percentage { get; set; }
        public int? LoadOrder { get; set; }
        public decimal? ToleranceMin { get; set; }
        public decimal? ToleranceMax { get; set; }
    }
}