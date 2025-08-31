namespace StorageSystem.Shared.Entities
{
    public class RecipeTotal
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string TotalRecipe { get; set; } = null!;
    }
}
