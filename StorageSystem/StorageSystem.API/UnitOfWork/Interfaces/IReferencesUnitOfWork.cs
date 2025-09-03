using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IReferencesUnitOfWork
    {
        Task<IEnumerable<Reference>> GetComboAsync();
        Task<IEnumerable<Reference>> GetWithTypeReferencesAndMeasurementUnitAsync();        
        Task<ActionResponse<IEnumerable<Reference>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);        
    }
}
