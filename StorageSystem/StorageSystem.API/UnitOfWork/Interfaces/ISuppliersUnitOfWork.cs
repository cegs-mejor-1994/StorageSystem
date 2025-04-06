using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface ISuppliersUnitOfWork
    {
        Task<IEnumerable<Supplier>> GetComboAsync();
    }
}
