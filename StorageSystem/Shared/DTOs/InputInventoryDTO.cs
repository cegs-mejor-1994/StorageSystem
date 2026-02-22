namespace StorageSystem.Shared.DTOs
{
    public class InputInventoryDTO
    {
        public int ID { get; set; }
        public string ControlCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public string ProductName { get; set; } = null!;
        public string MeasurementUnitCode { get; set; } = null!;

        public string Batch { get; set; } = null!;

        public DateTime MatutingDate { get; set; }

        public string SupplierName { get; set; } = null!;
    }
}
