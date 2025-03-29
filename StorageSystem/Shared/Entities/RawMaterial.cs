using StorageSystem.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class RawMaterial : IEntityFields
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

        public string State { get; set; } = "Disponible";
        public DateTime DateRegister { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int MeasurementUnitId { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }

        public ICollection<InputInventory>? InputInventories { get; set; }
    }
}
