using StorageSystem.API.Repositories.Implementations;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ProductionGapsUnitOfWork : GenericUnitOfWork<ProductionGap>, IProductionGapsUnitOfWork
    {
        private readonly IProductionGapsRepository _productionGapRepository;

        public ProductionGapsUnitOfWork(IGenericRepository<ProductionGap> repository, IProductionGapsRepository productionGapRepository) : base(repository)
        {
            _productionGapRepository = productionGapRepository;
        }

        public async Task<ActionResponse<IEnumerable<ProductionGap>>> GetAsync(PaginationDTO pagination) => await _productionGapRepository.GetAsync(pagination);

        public async Task<IEnumerable<ProductionGap>> GetComboAsync() => await _productionGapRepository.GetComboAsync();

        public async Task<ActionResponse<double>> GetProductionGapsByRecipeIdAsync(int recipeId) => await _productionGapRepository.GetProductionGapsByRecipeIdAsync(recipeId);

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _productionGapRepository.GetTotalPagesAsync(pagination);
    }
}
