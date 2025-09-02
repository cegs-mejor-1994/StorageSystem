using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ProductsDetailsRepository : GenericRepository<ProductsDetail>, IProductsDetailsRepository
    {
        private readonly DataContext _context;

        public ProductsDetailsRepository(DataContext context) : base(context)   
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<ProductsDetail>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.ProductsDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<ProductsDetail>>
            {
                WasSuccess = true,
                Result = await queryable
                 .OrderBy(r => r.Id)
                 .Include(r => r.RawMaterial)
                 .Include(r => r.AppearanceReference)
                 .Include(p => p.Product)
                 .ToListAsync()
            };
        }

        public async Task<IEnumerable<ProductsDetail>> GetComboAsync()
        {
            return await _context.ProductsDetails
                 .OrderBy(r => r.Id)
                 .Include(r => r.RawMaterial)
                 .Include(r => r.AppearanceReference)
                 .Include(p => p.Product)
                 .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.ProductsDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }
    }
}
