using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class ProductionGap
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Cantidad")]
        [Range(0.001, 9999999999, ErrorMessage = "El campo {0} debe ser mayor a 0 y menor que 9999999999")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Amount { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public int InputInventoryId { get; set; }
        public InputInventory? InputInventory { get; set; }

        public DateTime RegisterDate { get; set; }

        public ICollection<Manufactury>? Manufacturies { get; set; }
    }
}
