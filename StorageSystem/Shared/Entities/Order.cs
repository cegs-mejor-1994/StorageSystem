using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Total Pagar")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Amount { get; set; } = null!;

        public int ManuFacturyId { get; set; }
        public Manufactury? ManuFactury { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
