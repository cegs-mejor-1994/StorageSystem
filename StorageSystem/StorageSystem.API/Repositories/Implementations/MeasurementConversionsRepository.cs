using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class MeasurementConversionsRepository : GenericRepository<MeasurementConversion>, IMeasurementConversionsRepository
    {
        private readonly DataContext _context;

        public MeasurementConversionsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MeasurementConversion>> GetComboAsync()
        {
            return await _context.MeasurementConversions
                .OrderBy(mc => mc.Id)
                .Include(mc => mc.FromUnit)
                .Include(mc => mc.ToUnit)
                .ToListAsync();
        }
    }
}
