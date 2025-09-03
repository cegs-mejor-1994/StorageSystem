using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppearanceReferencesController : GenericController<AppearanceReference>
    {
        private readonly IAppearanceReferencesUnitOfWork _appearanceReferencesUnitOfWork;

        public AppearanceReferencesController(IGenericUnitOfWork<AppearanceReference> unitOfWork, IAppearanceReferencesUnitOfWork appearanceReferencesUnitOfWork) : base(unitOfWork)
        {
            _appearanceReferencesUnitOfWork = appearanceReferencesUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _appearanceReferencesUnitOfWork.GetComboAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _appearanceReferencesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _appearanceReferencesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
