using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IRawMaterialsUnitOfWork
    {
        Task<IEnumerable<RawMaterial>> GetComboAsync();
        Task<ActionResponse<IEnumerable<RawMaterial>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
