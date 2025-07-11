using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IManufacturiesUnitOfWork
    {
        Task<IEnumerable<Manufactury>> GetComboAsync();
    }
}
