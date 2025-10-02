using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ProductsDetailStructuresRepository : GenericRepository<ProductsDetailStructure>, IProductsDetailStructuresRepository
    {
        private readonly DataContext _context;

        public ProductsDetailStructuresRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductsDetailStructure>> GetComboAsync()
        {
            return await _context.ProductsDetailStructures
                .OrderBy(pds => pds.Id)
                .Include(rm => rm!.Product)
                .ThenInclude(c => c!.Category)
                .ToListAsync();
        }
    }
}
