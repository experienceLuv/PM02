namespace ProductionApi.DTOs
{
    public class RawMaterialLotCreateDto
    {
        public int RawMaterialId { get; set; }
        public string LotNumber { get; set; } = "";
        public string? Supplier { get; set; }
        public decimal Quantity { get; set; }
    }
}