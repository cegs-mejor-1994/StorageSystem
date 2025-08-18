using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class InputInventoriesRepository : GenericRepository<InputInventory>, IInputInventoriesRepository
    {
        private readonly DataContext _context;

        public InputInventoriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<InputInventory>>> GetAsync(PaginationDTO pagination)
        {                        
            var queryable = _context.InputInventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.RawMaterial!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<InputInventory>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Id)
                    .Include(i => i.RawMaterial!)
                    .ThenInclude(i => i.MeasurementUnit)
                    .Include(i => i.Supplier!)
                    .Paginate(pagination)
                    .ToListAsync()
            };
             
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.InputInventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.RawMaterial!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        public async Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync()
        {
            return await _context.InputInventories
                .OrderBy(i => i.RegisterDate)
                .Include(i => i.RawMaterial!)
                .ThenInclude(i => i.MeasurementUnit)
                .Include(i => i.Supplier!)                
                .ToListAsync();
        }
    }
}
