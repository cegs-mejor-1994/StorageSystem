using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class Recipe
    {
        public int Id { get; set; }        

        [Display(Name = "Cantidad")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Amount { get; set; } = null!;

        public int RawMaterialId { get; set; }
        public RawMaterial? RawMaterial { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string State { get; set; } = "Disponible";

        public ICollection<ProductionGap>? ProductionGaps { get; set; }
    }
}
