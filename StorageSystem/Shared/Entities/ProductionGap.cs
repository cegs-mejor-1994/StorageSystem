using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class ProductionGap
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Total Bache")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Amount { get; set; }

        [Display(Name = "Cantidad Restante")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LeftAmount { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }        
        
        public string State { get; set; } = "Disponible";

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        public ICollection<Manufactury>? Manufacturies { get; set; }
    }
}
