using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class Reference
    {
        public int Id { get; set; }

        [Display(Name = "Valor de Referencia")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]        
        public string Name { get; set; } = null!;

        public int MeasurementUnitId { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }

        public string State { get; set; } = "Disponible";

        public ICollection<AppearanceReference>? AppearanceReferences { get; set; }
    }
}
