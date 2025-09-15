using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class RecipeDetailsUnitOfWork : GenericUnitOfWork<RecipeDetail>, IRecipeDetailsUnitOfWork
    {
        private readonly IRecipeDetailsRepository _recipeDetailsRepository;

        public RecipeDetailsUnitOfWork(IGenericRepository<RecipeDetail> repository, IRecipeDetailsRepository recipeDetailsRepository) : base(repository)
        {
            _recipeDetailsRepository = recipeDetailsRepository;
        }

        public async Task<ActionResponse<IEnumerable<RecipeDetail>>> GetAsync(PaginationDTO pagination) => await _recipeDetailsRepository.GetAsync(pagination);

        public async Task<IEnumerable<RecipeDetail>> GetComboAsync() => await _recipeDetailsRepository.GetComboAsync();

        public async Task<ActionResponse<double>> GetTotalAmountOfRecipeDetails(int recipeID) => await _recipeDetailsRepository.GetTotalAmountOfRecipeDetails(recipeID);

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _recipeDetailsRepository.GetTotalPagesAsync(pagination);
    }
}
