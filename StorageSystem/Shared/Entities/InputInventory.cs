using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class InputInventory
    {
        public int Id { get; set; }

        [Display(Name = "Codigo de Control")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ControlCode { get; set; } = null!;        

        [Display(Name = "Cantidad")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.001, 9999999999, ErrorMessage = "El campo {0} debe ser mayor a 0 y menor que 9999999999")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Amount { get; set; }

        [Display(Name = "Cantidad Restante")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.001, 9999999999, ErrorMessage = "El campo {0} debe ser mayor a 0 y menor que 9999999999")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal LeftAmount { get; set; }

        [Display(Name = "Lote")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Batch { get; set; } = null!;

        [DataType(DataType.Date, ErrorMessage = "El campo {0} no tiene el formato especifico")]
        [Display(Name = "Fecha de Vencimiento")]        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime MatutingDate { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }  
        public Product? Product { get; set; } = null!;

        public ICollection<ProductionGapDetail>? ProductionGapDetails { get; set; }

        public string State { get; set; } = "Disponible";        
    }
}
