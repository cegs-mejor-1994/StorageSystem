using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class FormReference
    {
        public int Id { get; set; }         

        public int ReferenceId { get; set; }
        public Reference? Reference { get; set; }

        public int RawMaterialId { get; set; }
        public RawMaterial? RawMaterial { get; set; }

        public string State { get; set; } = "Disponible";

        public ICollection<ProductsDetail>? ProductsDetails { get; set; }
    }
}
