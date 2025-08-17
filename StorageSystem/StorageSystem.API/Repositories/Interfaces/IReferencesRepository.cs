using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IReferencesRepository
    {
        Task<IEnumerable<Reference>> GetComboAsync();
        Task<ActionResponse<IEnumerable<Reference>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
        Task<IEnumerable<Reference>> GetWithTypeReferencesAndMeasurementUnitAsync();
    }
}
