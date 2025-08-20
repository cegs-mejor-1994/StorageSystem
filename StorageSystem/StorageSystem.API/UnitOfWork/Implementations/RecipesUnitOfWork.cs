using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class RecipesUnitOfWork : GenericUnitOfWork<Recipe>, IRecipesUnitOfWork
    {
        private readonly IRecipesRepository _recipesRepository;

        public RecipesUnitOfWork(IGenericRepository<Recipe> repository, IRecipesRepository recipesRepository) : base(repository)
        {
            _recipesRepository = recipesRepository;
        }

        public async Task<ActionResponse<IEnumerable<Recipe>>> GetAsync(PaginationDTO pagination) => await _recipesRepository.GetAsync(pagination);

        public async Task<IEnumerable<Recipe>> GetComboAsync() => await _recipesRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _recipesRepository.GetTotalPagesAsync(pagination);
    }
}
