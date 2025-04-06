using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IRawMaterialsRepository
    {
        Task<IEnumerable<RawMaterial>> GetComboAsync();
    }
}
