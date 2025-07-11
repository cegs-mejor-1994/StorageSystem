using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class ProductionGap
    {
        public int Id { get; set; }

        [Display(Name = "Cantidad")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Amount { get; set; } = null!;

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public int InputInventoryId { get; set; }
        public InputInventory? InputInventory { get; set; }

        public ICollection<Manufactury>? Manufacturies { get; set; }
    }
}
