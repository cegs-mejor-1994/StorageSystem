using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IInputInventoriesUnitOfWork
    {
        Task<IEnumerable<InputInventoryDTO>> GetWithRawMaterialsAndSuppliersAsync();        
        Task<ActionResponse<IEnumerable<InputInventoryDTO>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
