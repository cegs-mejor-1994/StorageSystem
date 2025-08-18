using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IInputInventoriesUnitOfWork
    {
        Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync();        
        Task<ActionResponse<IEnumerable<InputInventory>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
