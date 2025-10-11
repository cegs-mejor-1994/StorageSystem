using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class Manufactury
    {
        public int Id { get; set; }

        [Display(Name = "Cantidad")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.001, 9999, ErrorMessage = "El campo {0} debe ser mayor a 0 y menor que 9999")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Amount { get; set; }

        public int ProductionGapId { get; set; }
        public ProductionGap? ProductionGap { get; set; }

        public int ProductsDetailId { get; set; }
        public ProductsDetail? ProductsDetail { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public string State { get; set; } = "Disponible";

        public DateTime RegisterDate { get; set; }
    }
}
