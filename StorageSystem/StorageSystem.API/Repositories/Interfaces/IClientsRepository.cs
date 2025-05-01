using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Client>> GetComboAsync();
    }
}
