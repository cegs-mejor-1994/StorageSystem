namespace StorageSystem.Shared.Entities
{
    public class ProductsDetail
    {
        public int Id { get; set; }
       
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        
        public int AppearanceReferenceId { get; set; }
        public AppearanceReference? AppearanceReference { get; set; }

        public int RawMaterialId { get; set; }
        public RawMaterial? RawMaterial { get; set; }

        public string State { get; set; } = "Disponible";
        public ICollection<Manufactury>? Manufacturies { get; set; }
    }
}
