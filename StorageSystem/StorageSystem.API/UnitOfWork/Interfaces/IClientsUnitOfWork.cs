using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IClientsUnitOfWork 
    {
        Task<IEnumerable<Client>> GetComboAsync();
    }
}
