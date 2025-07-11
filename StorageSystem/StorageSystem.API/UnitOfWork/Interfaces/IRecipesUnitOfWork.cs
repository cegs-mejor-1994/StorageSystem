using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IRecipesUnitOfWork
    {
        Task<IEnumerable<Recipe>> GetComboAsync();
    }
}
