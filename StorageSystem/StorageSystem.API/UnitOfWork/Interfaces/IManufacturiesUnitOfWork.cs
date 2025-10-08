using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IManufacturiesUnitOfWork
    {        
        Task<IEnumerable<Manufactury>> GetComboAsync();
        Task<ActionResponse<IEnumerable<Manufactury>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
