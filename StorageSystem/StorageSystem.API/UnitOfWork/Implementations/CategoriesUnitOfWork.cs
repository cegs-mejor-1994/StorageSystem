using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class CategoriesUnitOfWork : GenericUnitOfWork<Category>, ICategoriesUnitOfWork
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesUnitOfWork(IGenericRepository<Category> repository, ICategoriesRepository categoriesRepository): base(repository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination) => await _categoriesRepository.GetAsync(pagination);

        public async Task<ActionResponse<int>> GetCategoryById(string CCode, string CName) => await _categoriesRepository.GetCategoryById(CCode, CName);

        public async Task<IEnumerable<Category>> GetComboAsync() => await _categoriesRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _categoriesRepository.GetTotalPagesAsync(pagination);
    }
}
