using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IRecipeDetailsUnitOfWork
    {
        Task<IEnumerable<RecipeDetail>> GetComboAsync();
        Task<ActionResponse<double>> GetTotalAmountOfRecipeDetails(int recipeID);
        Task<ActionResponse<IEnumerable<RecipeDetail>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
        Task<ActionResponse<IEnumerable<RecipeDetail>>> FactorConversionInRecipeDetailsByRecipeId(int recipeID);
    }
}
