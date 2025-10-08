using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ManufacturiesRepository : GenericRepository<Manufactury>, IManufacturiesRepository
    {
        private readonly DataContext _context;

        public ManufacturiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<Manufactury>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Manufacturies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.ProductsDetail!.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Manufactury>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Id)
                    .Include(c => c.ProductsDetail)
                    .ThenInclude(pd => pd!.Product)
                    .Include(prod => prod.ProductsDetail)
                    .ThenInclude(r => r!.Reference)
                    .ThenInclude(r => r!.MeasurementUnit)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<Manufactury>> GetComboAsync()
        {
            return await _context.Manufacturies
                .OrderBy(m => m.Id)
                .Include(prod => prod.ProductsDetail)
                .ThenInclude(pd => pd!.Product)
                .Include(prod => prod.ProductsDetail)
                .ThenInclude(r => r!.Reference)
                .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Manufacturies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.ProductsDetail!.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
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
