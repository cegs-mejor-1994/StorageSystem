using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetComboAsync();
    }
}
