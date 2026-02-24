using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IInputInventoriesRepository
    {
        Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync();
        Task<ActionResponse<IEnumerable<InputInventoryDTO>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
