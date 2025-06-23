using System.ComponentModel.DataAnnotations;
namespace StorageSystem.Shared.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        //public ICollection<Recipe>? Recipes { get; set; } 
        [DataType(DataType.PhoneNumber)]
        public int ReferenceId { get; set; }
        public Reference? Reference { get; set; }
    }
}
