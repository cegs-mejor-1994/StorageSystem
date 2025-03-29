using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class MeasurementUnitsRepository : GenericRepository<MeasurementUnit>, IMeasurementUnitsRepository
    {
        private readonly DataContext _context;

        public MeasurementUnitsRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MeasurementUnit>> GetComboAsync()
        {
            return await _context.MeasurementUnits
                .OrderBy(m => m.Name)
                .ToListAsync(); 
        }
    }
}
