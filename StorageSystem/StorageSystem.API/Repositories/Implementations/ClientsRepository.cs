using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ClientsRepository : GenericRepository<Client>, IClientsRepository
    {
        private readonly DataContext _context;

        public ClientsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetComboAsync()
        {
            return await _context.Clients
                .OrderBy(c => c.Id)
                .ToListAsync();
        }
    }
}
