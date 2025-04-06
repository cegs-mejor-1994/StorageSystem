using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IInputInventoriesRepository
    {
        Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync();
    }
}
