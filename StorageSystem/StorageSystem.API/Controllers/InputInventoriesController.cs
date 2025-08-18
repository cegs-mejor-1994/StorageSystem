using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InputInventoriesController : GenericController<InputInventory>
    {
        private readonly IInputInventoriesUnitOfWork _inputInventoriesUnitOfWork;

        public InputInventoriesController(IGenericUnitOfWork<InputInventory> unitOfWork, IInputInventoriesUnitOfWork inputInventoriesUnitOfWork): base(unitOfWork)
        {
            _inputInventoriesUnitOfWork = inputInventoriesUnitOfWork;
        }

        [HttpGet("InputInventoryWithRawMaterialsAndSuppliers")]
        public async Task<IActionResult> GetInputInventoryWithRSAsync()
        {
            return Ok(await _inputInventoriesUnitOfWork.GetWithRawMaterialsAndSuppliersAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _inputInventoriesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _inputInventoriesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
