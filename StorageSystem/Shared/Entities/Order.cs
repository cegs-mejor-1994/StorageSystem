using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Total Pagar")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Amount { get; set; }

        public int ManuFacturyId { get; set; }
        public Manufactury? ManuFactury { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    }
}
