using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class SuppliersRepository : GenericRepository<Supplier>, ISuppliersRepository
    {
        private readonly DataContext _context;

        public SuppliersRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<Supplier>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Suppliers
                .Where(s => s.State == "Disponible")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => EF.Functions.Like(x.Name, $"%{pagination.Filter}%"));
            }

            return new ActionResponse<IEnumerable<Supplier>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<Supplier>> GetComboAsync()
        {
            return await _context.Suppliers
                .OrderBy(s => s.Name)
                .Where(s => s.State == "Disponible")
                .ToListAsync(); 
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Suppliers
                .Where(s => s.State == "Disponible")
                .AsQueryable();

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

        public async Task<ActionResponse<int>> GetSupplierById(string SNit)
        {
            try
            {
                var supplier = await _context.Suppliers
                    .FirstOrDefaultAsync(s => s.Nit == SNit && s.State == "Disponible");

                if (supplier == null)
                {
                    var existing = await _context.Suppliers.AnyAsync(s => s.Nit == SNit);

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
                        Message = "Ya existe un proveedor con estado Disponible con ese Código o Nombre."
                    };
                }

                bool existsActive = await _context.Suppliers.AnyAsync(s => s.State != "Eliminado" && s.Nit == SNit);

                if (existsActive)
                {
                    return new ActionResponse<int>
                    {
                        WasSuccess = true,
                        Result = -1,
                        Message = "El proveedor ya se encuentra registrado con estado diferente a Eliminado."
                    };
                }

                return new ActionResponse<int>
                {
                    WasSuccess = true,
                    Result = supplier.Id,
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
    }
}
