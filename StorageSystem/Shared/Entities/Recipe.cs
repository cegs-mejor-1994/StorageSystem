using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class Recipe
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Cantidad")]
        [Range(0.001, 9999999999, ErrorMessage = "El campo {0} debe ser mayor a 0 y menor que 9999999999")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Amount { get; set; }

        public int RawMaterialId { get; set; }
        public RawMaterial? RawMaterial { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string State { get; set; } = "Disponible";

        public ICollection<ProductionGap>? ProductionGaps { get; set; }
    }
}
