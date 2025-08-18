using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IProductsUnitOfWork
    {        
        Task<IEnumerable<Product>> GetComboAsync();
        Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
