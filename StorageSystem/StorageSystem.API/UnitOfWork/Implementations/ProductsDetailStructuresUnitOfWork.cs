using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ProductsDetailStructuresUnitOfWork: GenericUnitOfWork<ProductsDetailStructure>, IProductsDetailStructuresUnitOfWork
    {
        private readonly IProductsDetailStructuresRepository _productsDetailStructuresRepository;

        public ProductsDetailStructuresUnitOfWork(IGenericRepository<ProductsDetailStructure> repository, IProductsDetailStructuresRepository productsDetailStructuresRepository) : base(repository)
        {
            _productsDetailStructuresRepository = productsDetailStructuresRepository;
        }

        public async Task<IEnumerable<ProductsDetailStructure>> GetComboAsync(int productDetailID) => await _productsDetailStructuresRepository.GetComboAsync(productDetailID);
    }
}
