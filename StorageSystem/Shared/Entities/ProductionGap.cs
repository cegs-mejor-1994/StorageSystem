using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class ProductionGap
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Cantidad")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
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
