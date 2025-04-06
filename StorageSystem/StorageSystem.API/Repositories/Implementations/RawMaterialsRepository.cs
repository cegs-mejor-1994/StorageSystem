using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class RawMaterialsRepository : GenericRepository<RawMaterial>, IRawMaterialsRepository
    {
        private readonly DataContext _context;

        public RawMaterialsRepository(DataContext context): base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RawMaterial>> GetComboAsync()
        {
            return await _context.RawMaterials
                .OrderBy(rm => rm.Name)
                .ToListAsync();
        }
    }
}
