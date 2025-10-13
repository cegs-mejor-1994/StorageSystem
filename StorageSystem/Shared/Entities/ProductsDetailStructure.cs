using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class ProductsDetailStructure
    {
        public int Id { get; set; }

        public int ProductsDetailId { get; set; }
        public ProductsDetail? ProductDetail { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Unity { get; set; }

        public string State { get; set; } = "Disponible";

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    }
}
