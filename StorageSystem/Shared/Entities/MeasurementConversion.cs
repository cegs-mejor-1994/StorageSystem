using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class MeasurementConversion
    {
        public int Id { get; set; }

        public int FromUnitId { get; set; }
        public MeasurementUnit? FromUnit { get; set; } = null!;

        public int ToUnitId { get; set; }
        public MeasurementUnit? ToUnit { get; set; } = null!;

        [Column(TypeName = "decimal(18,4)")]
        public double Factor { get; set; }
    }
}
