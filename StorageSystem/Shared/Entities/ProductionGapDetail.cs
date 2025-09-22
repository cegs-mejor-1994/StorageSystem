using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class ProductionGapDetail
    {
        public int Id { get; set; }

        public int ProductionGapId { get; set; }
        public ProductionGap? ProductionGap { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; } = null!;

        public int MeasurementUnitId { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cantidad Utilizada")]
        [Range(0.001, 9999999999, ErrorMessage = "El campo {0} debe ser mayor a 0 y menor que 9999999999")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Amount { get; set; }

        public string State { get; set; } = "Disponible";
    }
}
