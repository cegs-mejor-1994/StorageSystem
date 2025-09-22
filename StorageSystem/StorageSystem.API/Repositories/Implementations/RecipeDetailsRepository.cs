using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class RecipeDetailsRepository : GenericRepository<RecipeDetail>, IRecipeDetailsRepository
    {
        private readonly DataContext _context;

        public RecipeDetailsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<RecipeDetail>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.RecipeDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<RecipeDetail>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(r => r.Id)
                    .Include(p => p.Product)
                    .Include(mu => mu.MeasurementUnit)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<RecipeDetail>> GetComboAsync()
        {
            return await _context.RecipeDetails
                .OrderBy(r => r.Id)
                .Include(p => p.Product)
                .Include(mu => mu.MeasurementUnit)
                .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.RecipeDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        public async Task<ActionResponse<double>> GetTotalAmountOfRecipeDetails(int recipeID)
        {
            double totalRecipe = 0;
            var productRecipeDetails = await _context.RecipeDetails.Where(rd => rd.RecipeId == recipeID)
                .Include(rd => rd.MeasurementUnit)
                .ToListAsync(); 

            if (productRecipeDetails == null || productRecipeDetails.Count == 0)
            {
                return new ActionResponse<double>
                {
                    WasSuccess = true,
                    Result = 0
                };
            }

            foreach (var productRecipeDetail in productRecipeDetails)
            {
                var currenUnit = productRecipeDetail.MeasurementUnit;

                var baseUnit = await _context.MeasurementUnits.Where(mu => mu.PhysicalState == currenUnit!.PhysicalState && mu.Base).FirstAsync();

                var factorConversion = await _context.MeasurementConversions.Where(cf => cf.FromUnitId == currenUnit!.Id && cf.ToUnitId == baseUnit.Id).Select(cf => cf.Factor).FirstOrDefaultAsync();

                var amountBase = (double)productRecipeDetail.Amount * factorConversion;

                totalRecipe += (double)amountBase;
            }

            return new ActionResponse<double>
            {
                WasSuccess = true,
                Result = totalRecipe
            };

        }

        public async Task<ActionResponse<IEnumerable<RecipeDetail>>> FactorConversionInRecipeDetailsByRecipeId(int recipeID)
        {
            List<RecipeDetail>? recipeDetails = new List<RecipeDetail>();
            var productRecipeDetails = await _context.RecipeDetails
                    .Where(rd => rd.RecipeId == recipeID)
                    .Include(rd => rd.MeasurementUnit)
                    .Include(pr => pr.Product)
                    .ToListAsync();

            if (productRecipeDetails == null || productRecipeDetails.Count == 0)
            {
                return new ActionResponse<IEnumerable<RecipeDetail>>
                {
                    WasSuccess = true,
                    Result = new List<RecipeDetail>()
                };
            }            

            foreach (var productRecipeDetail in productRecipeDetails)
            {
                var currenUnit = productRecipeDetail.MeasurementUnit;
                if (currenUnit == null)
                {
                    return new ActionResponse<IEnumerable<RecipeDetail>>
                    {
                        WasSuccess = false,
                        Message = "El detalle no tiene unidad de medida asignada."
                    };
                }

                var baseUnit = await _context.MeasurementUnits
                    .Where(mu => mu.PhysicalState == currenUnit.PhysicalState && mu.Base)
                    .FirstOrDefaultAsync();

                if (baseUnit == null)
                {
                    return new ActionResponse<IEnumerable<RecipeDetail>>
                    {
                        WasSuccess = false,
                        Message = $"No hay unidad base para el estado {currenUnit.PhysicalState}."
                    };
                }

                var factorConversion = await _context.MeasurementConversions
                    .Where(cf => cf.FromUnitId == currenUnit.Id && cf.ToUnitId == baseUnit.Id)
                    .Select(cf => cf.Factor)
                    .FirstOrDefaultAsync();

                if (factorConversion == 0)
                {
                    return new ActionResponse<IEnumerable<RecipeDetail>>
                    {
                        WasSuccess = false,
                        Message = $"No hay factor de conversión de {currenUnit.Name} a {baseUnit.Name}."
                    };
                }

                productRecipeDetail.Amount = (decimal)((double)productRecipeDetail.Amount * factorConversion);
                recipeDetails.Add(productRecipeDetail);
            }            

            return new ActionResponse<IEnumerable<RecipeDetail>>
            {
                WasSuccess = true,
                Result = recipeDetails
            };
        }
    }
}
