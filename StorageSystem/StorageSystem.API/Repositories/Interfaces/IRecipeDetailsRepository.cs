using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IRecipeDetailsRepository
    {
        Task<IEnumerable<RecipeDetail>> GetComboAsync();
        Task<ActionResponse<IEnumerable<RecipeDetail>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
