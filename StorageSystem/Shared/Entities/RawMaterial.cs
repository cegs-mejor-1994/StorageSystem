using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class RawMaterial
    {
        [Key]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public string State { get; set; } = "Disponible";
    }
}
