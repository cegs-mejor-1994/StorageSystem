namespace StorageSystem.Shared.Entities
{
    public class ProductionGapDetail
    {
        public int Id { get; set; }

        public int ProductionGapId { get; set; }
        public ProductionGap? ProductionGap { get; set; }

        public int InputInventoryId { get; set; }
        public InputInventory? InputInventory { get; set; } = null!;

        public string State { get; set; } = "Disponible";
    }
}
