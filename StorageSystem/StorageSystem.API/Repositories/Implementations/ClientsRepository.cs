using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ClientsRepository : GenericRepository<Client>, IClientsRepository
    {
        private readonly DataContext _context;

        public ClientsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Clients
                .Where(cl => cl.State == "Disponible")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => EF.Functions.Like(x.Name, $"%{pagination.Filter}%"));
            }

            return new ActionResponse<IEnumerable<Client>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<int>> GetClientById(string CNit)
        {
            try
            {
                var client = await _context.Clients
                    .FirstOrDefaultAsync(cl => cl.Nit == CNit && cl.State == "Disponible");

                if (client == null)
                {
                    var existing = await _context.Clients.AnyAsync(cl => cl.Nit == CNit);

                    if (!existing)
                    {
                        return new ActionResponse<int>
                        {
                            WasSuccess = true,
                            Result = 0,
                            Message = "Se puede crear el registro"
                        };
                    }

                    return new ActionResponse<int>
                    {
                        WasSuccess = true,
                        Result = -1,
                        Message = "Ya existe un cliente con estado Disponible con ese Nit."
                    };
                }

                bool existsActive = await _context.Suppliers.AnyAsync(cl => cl.State != "Eliminado" && cl.Nit == CNit);

                if (existsActive)
                {
                    return new ActionResponse<int>
                    {
                        WasSuccess = true,
                        Result = -1,
                        Message = "El cliente ya se encuentra registrado con estado diferente a Eliminado."
                    };
                }

                return new ActionResponse<int>
                {
                    WasSuccess = true,
                    Result = client.Id,
                    Message = "Se puede cambiar el estado."
                };
            }
            catch (Exception ex)
            {
                return new ActionResponse<int>
                {
                    WasSuccess = false,
                    Result = -99,
                    Message = $"Error al realizar la operacion: {ex.Message}"
                };
            }        
        }

        public async Task<IEnumerable<Client>> GetComboAsync()
        {
            return await _context.Clients.Where(cl => cl.State == "Disponible")
                .OrderBy(c => c.Id)
                .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Clients.Where(cl => cl.State == "Disponible").AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => EF.Functions.Like(x.Name, $"%{pagination.Filter}%"));
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
