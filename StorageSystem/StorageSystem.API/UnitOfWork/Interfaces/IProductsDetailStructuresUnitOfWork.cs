using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IProductsDetailStructuresUnitOfWork
    {
        Task<IEnumerable<ProductsDetailStructure>> GetComboAsync();
    }
}
