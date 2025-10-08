namespace StorageSystem.Shared.DTOs
{
    public class ManufacturyDTO
    {
        public int ProductionGapID { get; set; }        
        public int RecipeID { get; set; }
        public decimal RecipeLeftAmount { get; set; }
    }
}
