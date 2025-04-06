using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class InputInventory
    {
        public int Id { get; set; }

        [Display(Name = "Cantidad")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Amount { get; set; } = null!;

        [Display(Name = "Lote")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Batch { get; set; } = null!;

        [DataType(DataType.Date, ErrorMessage = "El campo {0} no tiene el formato especifico")]
        [Display(Name = "Fecha de Vencimiento")]        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime MatutingDate { get; set; }

        public DateTime RegisterDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int RawMaterialId { get; set; }
        public RawMaterial? RawMaterial { get; set; }        
    }
}
