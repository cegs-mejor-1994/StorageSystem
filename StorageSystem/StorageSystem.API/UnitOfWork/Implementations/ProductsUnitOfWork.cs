using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ProductsUnitOfWork : GenericUnitOfWork<Product>, IProductsUnitOfWork
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsUnitOfWork(IGenericRepository<Product> repository, IProductsRepository productsRepository) : base(repository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination) => await _productsRepository.GetAsync(pagination);

        public async Task<IEnumerable<Product>> GetComboAsync() => await _productsRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _productsRepository.GetTotalPagesAsync(pagination);

    }
}
