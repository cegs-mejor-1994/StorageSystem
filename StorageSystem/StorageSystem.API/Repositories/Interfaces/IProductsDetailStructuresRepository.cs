using StorageSystem.Shared.DTOs;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IProductsDetailStructuresRepository
    {
        Task<IEnumerable<ProductDetailStructureDTO>> GetComboAsync(int productDetailID);
    }
}
