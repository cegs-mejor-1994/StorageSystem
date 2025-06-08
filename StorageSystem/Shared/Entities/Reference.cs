using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageSystem.Shared.Entities
{
    public class Reference
    {
        public int Id { get; set; }

        [Display(Name = "Referencia")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string Name { get; set; } = null!;

        [Display(Name = "Tipo de Referencia")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ReferenceType { get; set; } = null!;

        public ICollection<Product>? Products { get; set; }
    }
}
