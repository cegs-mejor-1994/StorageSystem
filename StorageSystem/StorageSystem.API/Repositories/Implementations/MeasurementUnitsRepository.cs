using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class MeasurementUnitsRepository : GenericRepository<MeasurementUnit>, IMeasurementUnitsRepository
    {
        private readonly DataContext _context;

        public MeasurementUnitsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<MeasurementUnit>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.MeasurementUnits.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<MeasurementUnit>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)                    
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<MeasurementUnit>> GetComboAsync()
        {
            return await _context.MeasurementUnits
                .OrderBy(x => x.Name)                
                .ToListAsync(); 
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.MeasurementUnits.AsQueryable();

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

        public async Task<ActionResponse<double>> GetBaseUnitWithFactor(string physycalState, int meausementUnitFactorId)
        {  
            var baseUnit = await _context.MeasurementUnits.Where(mu => mu.PhysicalState.ToString() == physycalState && mu.Base).FirstAsync();

            var factorConversion = await _context.MeasurementConversions.Where(cf => cf.FromUnitId == meausementUnitFactorId && cf.ToUnitId == baseUnit.Id).Select(cf => cf.Factor).FirstOrDefaultAsync();
            
            return new ActionResponse<double>
            {
                WasSuccess = true,
                Result = factorConversion
            };
        }
    }
}
