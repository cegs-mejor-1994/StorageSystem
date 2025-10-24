using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetComboAsync();
        Task<ActionResponse<int>> GetCategoryById(string CCode, string CName);
        Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
