using StorageSystem.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.Shared.Entities
{
    public class Product : IEntityFields
    {
        public int Id { get; set; }

        [Display(Name = "Codigo")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Code { get; set; } = null!;

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = null!;

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public ProductPhysicalState PhysicalState { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public ProductRole Role { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        public string State { get; set; } = "Disponible";
        
        public RawMaterial? RawMaterial { get; set; }

        public ICollection<ProductsDetail>? ProductsDetails { get; set; }
        public ICollection<InputInventory>? InputInventories { get; set; }
        public ICollection<Recipe>? Recipes { get; set; }
        public ICollection<RecipeDetail>? RecipeDetails { get; set; }
        
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
