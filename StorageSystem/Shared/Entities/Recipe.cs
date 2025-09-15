using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class Recipe
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Total Bache")]        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal TotalRecipe { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string State { get; set; } = "Disponible";

        public ICollection<ProductionGap>? ProductionGaps { get; set; }
        public ICollection<RecipeDetail>? RecipeDetails { get; set; }
    }
}
