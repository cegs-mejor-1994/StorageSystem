using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IInputInventoriesUnitOfWork
    {
        Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync();
    }
}
