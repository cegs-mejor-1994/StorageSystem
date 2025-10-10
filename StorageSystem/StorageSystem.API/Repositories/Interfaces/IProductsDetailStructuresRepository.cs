using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IProductsDetailStructuresRepository
    {
        Task<IEnumerable<ProductsDetailStructure>> GetComboAsync(int productDetailID);
    }
}
