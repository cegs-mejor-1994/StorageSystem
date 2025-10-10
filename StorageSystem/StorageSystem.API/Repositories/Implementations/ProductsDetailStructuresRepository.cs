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

        public async Task<IEnumerable<ProductsDetailStructure>> GetComboAsync(int productDetailID)
        {
            return await _context.ProductsDetailStructures
                .Include(pds => pds.Product)
                .ThenInclude(cat => cat!.Category)
                .Where(prDeS => prDeS.ProductsDetailId ==  productDetailID)
                .OrderBy(pds => pds.Id)
                .ToListAsync();
        }
    }
}
