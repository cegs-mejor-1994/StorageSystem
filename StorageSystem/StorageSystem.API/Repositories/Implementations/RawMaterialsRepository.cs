using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class RawMaterialsRepository : GenericRepository<RawMaterial>, IRawMaterialsRepository
    {
        private readonly DataContext _context;

        public RawMaterialsRepository(DataContext context): base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<RawMaterial>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.RawMaterials.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<RawMaterial>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Product!.Name)
                    .Include(i => i.Supplier!)
                    .Include(i => i.Product!)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<RawMaterial>> GetComboAsync()
        {
            return await _context.RawMaterials
                .OrderBy(x => x.Product!.Name)
                .Include(i => i.Supplier!)
                .Include(i => i.Product!)
                .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.RawMaterials.AsQueryable();

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
