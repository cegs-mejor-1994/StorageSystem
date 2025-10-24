using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        private readonly DataContext _context;

        public CategoriesRepository(DataContext context): base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Categories
                .Where(c => c.State == "Disponible")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => EF.Functions.Like(x.Name, $"%{pagination.Filter}%"));
            }

            return new ActionResponse<IEnumerable<Category>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<Category>> GetComboAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .Where(c => c.State == "Disponible")
                .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Categories
                .Where(c => c.State == "Disponible")
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

        public async Task<ActionResponse<int>> GetCategoryById(string CCode, string CName)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Code == CCode && c.Name == CName && c.State == "Disponible");

                if (category == null)
                {
                    var existing = await _context.Categories.AnyAsync(c => c.Code == CCode || c.Name == CName);

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
                        Message = "Ya existe una categoria con estado Disponible con ese Código o Nombre."
                    };
                }

                bool existsActive = await _context.Categories.AnyAsync(c => c.State != "Eliminado" && (c.Code == CCode || c.Name == CName));

                if (existsActive) 
                {
                    return new ActionResponse<int>
                    {
                        WasSuccess = true,
                        Result = -1,
                        Message = "Ya existe una categoria con estado Disponible con ese Código o Nombre."
                    };
                }

                return new ActionResponse<int>
                {
                    WasSuccess = true,
                    Result = category.Id,
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
