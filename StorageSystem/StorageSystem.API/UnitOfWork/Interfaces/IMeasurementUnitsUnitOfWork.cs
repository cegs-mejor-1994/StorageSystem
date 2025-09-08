using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IMeasurementUnitsUnitOfWork
    {        
        Task<IEnumerable<MeasurementUnit>> GetComboAsync();
        Task<ActionResponse<IEnumerable<MeasurementUnit>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
        
    }
}
