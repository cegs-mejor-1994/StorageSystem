using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class AppearanceReferencesRepository : GenericRepository<AppearanceReference>, IAppearanceReferencesRepository
    {
        private readonly DataContext _context;
        public AppearanceReferencesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<AppearanceReference>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.AppearanceReferences.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Reference!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            return new ActionResponse<IEnumerable<AppearanceReference>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(ar => ar.Id)
                    .Include(ar => ar.Reference)
                    .ThenInclude(m => m!.MeasurementUnit)
                    .Include(ar => ar.RawMaterial)
                    .ThenInclude(ca => ca!.Category)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<AppearanceReference>> GetComboAsync()
        {
            return await _context.AppearanceReferences
                    .OrderBy(ar => ar.Id)
                    .Include(ar => ar.Reference)
                    .ThenInclude(m => m!.MeasurementUnit)
                    .Include(ar => ar.RawMaterial)
                    .ThenInclude(ca => ca!.Category)
                    .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.AppearanceReferences.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Reference!.Name.ToLower().Contains(pagination.Filter.ToLower()));
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
