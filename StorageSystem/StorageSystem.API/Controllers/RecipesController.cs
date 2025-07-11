using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
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
    }
}
