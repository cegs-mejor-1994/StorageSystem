using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IProductsUnitOfWork
    {
        Task<IEnumerable<Product>> GetComboAsync();
    }
}
