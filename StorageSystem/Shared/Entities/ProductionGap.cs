using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class ProductionGap
    {
        public int Id { get; set; }
        
        [Display(Name = "Cantidad")]
        [Range(0.001, 9999999999, ErrorMessage = "El campo {0} debe ser mayor a 0 y menor que 9999999999")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Amount { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }        
        
        public string State { get; set; } = "Disponible";

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        public ICollection<Manufactury>? Manufacturies { get; set; }
    }
}
