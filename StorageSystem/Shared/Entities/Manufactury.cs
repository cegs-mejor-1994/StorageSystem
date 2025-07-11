using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class Manufactury
    {
        public int Id { get; set; }

        [Display(Name = "Cantidad")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Amount { get; set; } = null!;

        public int ProductionGapId { get; set; }
        public ProductionGap? ProductionGap { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
