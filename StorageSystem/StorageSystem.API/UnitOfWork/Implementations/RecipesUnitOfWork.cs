using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class RecipesUnitOfWork : GenericUnitOfWork<Recipe>, IRecipesUnitOfWork
    {
        private readonly IRecipesRepository _recipesRepository;

        public RecipesUnitOfWork(IGenericRepository<Recipe> repository, IRecipesRepository recipesRepository) : base(repository)
        {
            _recipesRepository = recipesRepository;
        }

        public async Task<IEnumerable<Recipe>> GetComboAsync() => await _recipesRepository.GetComboAsync();
    }
}
