using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ProductsDetailsUnitOfWork : GenericUnitOfWork<ProductsDetail>, IProductsDetailsUnitOfWork
    {
        private readonly IProductsDetailsRepository _productsDetailsRepository;

        public ProductsDetailsUnitOfWork(IGenericRepository<ProductsDetail> repository, IProductsDetailsRepository productsDetailsRepository) : base(repository)
        {
            _productsDetailsRepository = productsDetailsRepository;
        }

        public async Task<ActionResponse<IEnumerable<ProductsDetail>>> GetAsync(PaginationDTO pagination) => await _productsDetailsRepository.GetAsync(pagination);

        public async Task<IEnumerable<ProductsDetail>> GetComboAsync() => await _productsDetailsRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _productsDetailsRepository.GetTotalPagesAsync(pagination);
    }
}
