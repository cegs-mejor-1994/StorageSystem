using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IProductionGapRepository
    {
        Task<IEnumerable<ProductionGap>> GetComboAsync();
        Task<ActionResponse<IEnumerable<ProductionGap>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
