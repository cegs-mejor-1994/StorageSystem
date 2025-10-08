using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IProductsDetailsUnitOfWork
    {
        Task<IEnumerable<ProductsDetail>> GetComboAsync();
        Task<IEnumerable<ProductDetailDTO>> GetWithReferencesAsync();
        Task<ActionResponse<ManufacturyDTO>> GetProductDetailGapRecipe(int ProductID);
        Task<ActionResponse<IEnumerable<ProductsDetail>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
