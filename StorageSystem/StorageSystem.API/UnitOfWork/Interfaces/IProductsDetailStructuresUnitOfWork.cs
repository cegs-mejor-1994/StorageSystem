using StorageSystem.Shared.DTOs;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IProductsDetailStructuresUnitOfWork
    {
        Task<IEnumerable<ProductDetailStructureDTO>> GetComboAsync(int productDetailID);
    }
}
