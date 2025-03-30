using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface ISuppliersRepository
    {
        Task<IEnumerable<Supplier>> GetComboAsync();
    }
}
