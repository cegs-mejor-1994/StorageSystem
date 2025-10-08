using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : GenericController<Recipe>
    {
        private readonly IRecipesUnitOfWork _recipesUnitOfWork;

        public RecipesController(IGenericUnitOfWork<Recipe> unitOfWork, IRecipesUnitOfWork recipesUnitOfWork) : base(unitOfWork)
        {
            _recipesUnitOfWork = recipesUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _recipesUnitOfWork.GetComboAsync());
        }

        [HttpGet("comboForManufactury")]
        public async Task<IActionResult> GetComboForManufacturyAsync()
        {
            return Ok(await _recipesUnitOfWork.GetComboForManufacturyAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _recipesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _recipesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
