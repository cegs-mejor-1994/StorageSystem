using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class Recipe
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Cantidad")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
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
