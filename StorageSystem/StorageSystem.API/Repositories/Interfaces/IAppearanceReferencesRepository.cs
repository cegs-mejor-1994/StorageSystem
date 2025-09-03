using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IAppearanceReferencesRepository
    {
        Task<IEnumerable<AppearanceReference>> GetComboAsync();
        Task<ActionResponse<IEnumerable<AppearanceReference>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
