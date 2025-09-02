using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class RecipeTotal
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal TotalRecipe { get; set; }
    }
}
