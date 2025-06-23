using System.ComponentModel.DataAnnotations;
namespace StorageSystem.Shared.Entities
{
    public class Product
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

        [Display(Name = "Tipo")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string Type { get; set; } = null!;
        //public ICollection<Recipe>? Recipes { get; set; }         
        //public int ReferenceId { get; set; }
        //public Reference? Reference { get; set; }
    }
}
