using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ReferencesRepository : GenericRepository<Reference>, IReferencesRepository
    {
        private readonly DataContext _context;

        public ReferencesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<Reference>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.References.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Reference>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Include(r => r.MeasurementUnit)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<Reference>> GetComboAsync()
        {
            return await _context.References
                .OrderBy(r => r.Name)
                .Include(m => m.MeasurementUnit)
                .Select(r => new Reference
                {
                    Id = r.Id,
                    Name = r.Name,
                    MeasurementUnitId = r.MeasurementUnitId,
                    MeasurementUnit = new MeasurementUnit
                    {                        
                        Name = r.Name + " " + r.MeasurementUnit!.Code
                    }
                })
                .ToListAsync();                
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.References.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        public async Task<IEnumerable<Reference>> GetWithTypeReferencesAndMeasurementUnitAsync()
        {
            return await _context.References
                .OrderBy(r => r.Name)                
                .Include(r => r.MeasurementUnit)
                .ToListAsync();
        }
    }
}
