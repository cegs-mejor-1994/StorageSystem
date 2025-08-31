using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IProductionGapsUnitOfWork
    {
        Task<IEnumerable<ProductionGap>> GetComboAsync();
        Task<ActionResponse<IEnumerable<ProductionGap>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
