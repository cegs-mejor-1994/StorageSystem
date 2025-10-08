using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.Shared.DTOs
{
    public class ProductDetailDTO
    {
        public int ProductDetailID { get; set; }
        public int productID { get; set; }
        public string FullName { get; set; } = null!;
        public string ReferenceValue { get; set; } = null!;
        public int MeasurementUnitID { get; set; }
        public ProductPhysicalState PhysicalState { get; set; }
        public string State { get; set; } = null!;
    }
}
