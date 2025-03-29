using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface ICategoriesUnitOfWork
    {
        Task<IEnumerable<Category>> GetComboAsync();
    }
}
