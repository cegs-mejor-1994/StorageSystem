using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IReferencesUnitOfWork
    {
        Task<IEnumerable<Reference>> GetComboAsync();
    }
}
