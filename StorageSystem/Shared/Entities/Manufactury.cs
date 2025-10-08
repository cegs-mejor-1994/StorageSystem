using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class Manufactury
    {
        public int Id { get; set; }

        public string ControlCode { get; set; } = null!;

        [Display(Name = "Cantidad")]        
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Amount { get; set; }

        public int ProductionGapId { get; set; }
        public ProductionGap? ProductionGap { get; set; }

        public int ProductsDetailId { get; set; }
        public ProductsDetail? ProductsDetail { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public string State { get; set; } = "Disponible";

        public DateTime RegisterDate { get; set; }
    }
}
