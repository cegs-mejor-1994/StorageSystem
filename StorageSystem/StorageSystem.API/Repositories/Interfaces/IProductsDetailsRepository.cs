using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IProductsDetailsRepository
    {
        Task<IEnumerable<ProductsDetail>> GetComboAsync();
        Task<ActionResponse<IEnumerable<ProductsDetail>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
