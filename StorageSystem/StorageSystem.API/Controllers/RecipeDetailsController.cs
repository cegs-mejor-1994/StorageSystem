using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeDetailsController : GenericController<RecipeDetail>
    {
        private readonly IRecipeDetailsUnitOfWork _recipeDetailsUnitOfWork;

        public RecipeDetailsController(IGenericUnitOfWork<RecipeDetail> unitOfWork, IRecipeDetailsUnitOfWork recipeDetailsUnitOfWork) : base(unitOfWork)
        {
            _recipeDetailsUnitOfWork = recipeDetailsUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _recipeDetailsUnitOfWork.GetComboAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _recipeDetailsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _recipeDetailsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalAmount")]
        public async Task<IActionResult> GetTotalAmountAsync(int recipeID)
        {
            var action = await _recipeDetailsUnitOfWork.GetTotalAmountOfRecipeDetails(recipeID);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("FactorConversionDetails")]
        public async Task<IActionResult> GetRecipeDetailsConversion(int recipeID)
        {
            var action = await _recipeDetailsUnitOfWork.FactorConversionInRecipeDetailsByRecipeId(recipeID);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
