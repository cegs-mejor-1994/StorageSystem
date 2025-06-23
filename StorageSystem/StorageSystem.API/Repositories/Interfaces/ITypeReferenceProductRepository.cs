using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface ITypeReferenceProductRepository
    {
        Task<IEnumerable<TypeReferenceProduct>> GetComboAsync();
    }
}
