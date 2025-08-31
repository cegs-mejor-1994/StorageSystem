namespace StorageSystem.Shared.Entities
{
    public  class ProductsDetail
    {
        public int Id { get; set; }
       
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int ReferenceId { get; set; }
        public Reference? Reference { get; set; }

        public string State { get; set; } = "Disponible";
        public ICollection<Manufactury>? Manufacturies { get; set; }
    }
}
