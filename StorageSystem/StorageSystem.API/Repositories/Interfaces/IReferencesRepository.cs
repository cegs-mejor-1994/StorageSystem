using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IReferencesRepository
    {
        Task<IEnumerable<Reference>> GetComboAsync();
    }
}
