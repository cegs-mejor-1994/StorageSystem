using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageSystem.Shared.Entities
{
    public class ProductionGapDetail
    {
        public int Id { get; set; }

        public int ProductionGapId { get; set; }
        public ProductionGap? ProductionGap { get; set; }       
        public int? InputInventoryId { get; set; }
        public InputInventory? InputInventory { get; set; }
        public int? ProductionGapSourceId { get; set; }
        public ProductionGap? ProductionGapSource { get; set; }

        [Display(Name = "Cantidad")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string State { get; set; } = "Disponible";
    }
}
