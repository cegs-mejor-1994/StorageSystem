using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IRawMaterialsUnitOfWork
    {
        Task<IEnumerable<RawMaterial>> GetComboAsync();
    }
}
