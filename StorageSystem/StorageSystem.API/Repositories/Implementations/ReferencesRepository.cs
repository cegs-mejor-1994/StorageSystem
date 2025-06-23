using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ReferencesRepository : GenericRepository<Reference>, IReferencesRepository
    {
        private readonly DataContext _context;

        public ReferencesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reference>> GetComboAsync()
        {
            return await _context.References
                .OrderBy(r => r.Name)
                .ToListAsync();
        }
    }
}
