using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface ITypeReferenceProductUnitOfWork
    {
        Task<IEnumerable<TypeReferenceProduct>> GetComboAsync();
    }
}
