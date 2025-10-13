using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class MeasurementConversion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int FromUnitId { get; set; }
        public MeasurementUnit? FromUnit { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ToUnitId { get; set; }
        public MeasurementUnit? ToUnit { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Column(TypeName = "decimal(18,4)")]
        public double Factor { get; set; }
    }
}
