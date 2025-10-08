using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IRecipesRepository
    {
        Task<IEnumerable<Recipe>> GetComboAsync();
        Task<IEnumerable<Recipe>> GetComboForManufacturyAsync();
        Task<ActionResponse<IEnumerable<Recipe>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
