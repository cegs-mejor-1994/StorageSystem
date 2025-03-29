using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        [Display(Name = "Identificacion")]
        [MaxLength(15, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nit { get; set; } = null!;

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = null!;

        [Display(Name = "Direccion")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Address { get; set; } = null!;

        [Display(Name = "Telefono")]
        [MaxLength(12, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Phone { get; set; } = null!;

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string City { get; set; } = null!;

        public ICollection<InputInventory>? InputInventories { get; set; } 
    }
}
