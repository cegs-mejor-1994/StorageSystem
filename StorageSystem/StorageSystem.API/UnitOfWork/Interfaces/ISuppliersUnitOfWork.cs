using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface ISuppliersUnitOfWork
    {        
        Task<IEnumerable<Supplier>> GetComboAsync();
        Task<ActionResponse<IEnumerable<Supplier>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);           
    }
}
