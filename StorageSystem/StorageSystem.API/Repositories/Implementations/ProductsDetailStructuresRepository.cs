using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ProductsDetailStructuresRepository : GenericRepository<ProductsDetailStructure>, IProductsDetailStructuresRepository
    {
        private readonly DataContext _context;

        public ProductsDetailStructuresRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDetailStructureDTO>> GetComboAsync(int productDetailID)
        {
            return await _context.ProductsDetailStructures
                .Where(prDeS => prDeS.ProductsDetailId ==  productDetailID)
                .OrderBy(pds => pds.Id)
                .Select(pds => new ProductDetailStructureDTO
                {
                    Id = pds.Id,
                    Name = pds.Product!.Name
                })
                .ToListAsync();
        }
    }
}
