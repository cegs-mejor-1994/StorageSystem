using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IRerefencesRepository
    {
        Task<IEnumerable<Reference>> GetComboAsync();
    }
}
