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
                    .Where(ms => ms.State == "Disponible")
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<MeasurementUnit>> GetComboAsync()
        {
            return await _context.MeasurementUnits
                .OrderBy(x => x.Name)
                .Where(ms => ms.State == "Disponible")
                .ToListAsync(); 
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.MeasurementUnits
                .Where(ms => ms.State == "Disponible")
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

        public async Task<ActionResponse<int>> GetMeasurementUnitById(string MCode, string MName)
        {
            try
            {
                // Escenario 1: Buscar si existe registro eliminado
                var mUnit = await _context.MeasurementUnits
                       .FirstOrDefaultAsync(mu => mu.Code == MCode && mu.Name == MName && mu.State == "Eliminado");

                // Escenario 2: Si no existe eliminado, verificar si existe activo o en otro estado
                if (mUnit == null)
                {
                    var existing = await _context.MeasurementUnits
                        .AnyAsync(mu => mu.Code == MCode || mu.Name == MName);

                    // Escenario 2A: No existe ningún registro → crear uno nuevo
                    if (!existing)
                    {
                        return new ActionResponse<int>
                        {
                            WasSuccess = true,
                            Result = 0,
                            Message = "Se puede crear el registro."
                        };
                    }

                    // Escenario 2B: No existe eliminado, pero sí existe otro (activo o diferente)
                    return new ActionResponse<int>
                    {
                        WasSuccess = true,
                        Result = -1,
                        Message = "Ya existe una unidad de medida con estado Disponible con ese Código o Nombre."
                    };
                }

                // Escenario 3: Si existe eliminado, validar si puede reactivarse
                bool existsActive = await _context.MeasurementUnits.AnyAsync(mu =>
                    mu.State != "Eliminado" &&
                    (mu.Code == MCode || mu.Name == MName));

                if (existsActive)
                {
                    return new ActionResponse<int>
                    {
                        WasSuccess = true,
                        Result = -1,
                        Message = "Ya existe una unidad de medida con estado Disponible con ese Código o Nombre."
                    };
                }

                // Escenario 4: Reactivar registro eliminado
                return new ActionResponse<int>
                {
                    WasSuccess = true,
                    Result = mUnit.Id,
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

        public async Task<ActionResponse<double>> GetBaseUnitWithFactor(string physycalState, int meausementUnitFactorId)
        {  
            var baseUnit = await _context.MeasurementUnits.Where(mu => mu.PhysicalState.ToString() == physycalState && mu.Base && mu.State == "Disponible").FirstAsync();

            var factorConversion = await _context.MeasurementConversions.Where(cf => cf.FromUnitId == meausementUnitFactorId && cf.ToUnitId == baseUnit.Id).Select(cf => cf.Factor).FirstOrDefaultAsync();
            
            return new ActionResponse<double>
            {
                WasSuccess = true,
                Result = factorConversion
            };
        }
    }
}
