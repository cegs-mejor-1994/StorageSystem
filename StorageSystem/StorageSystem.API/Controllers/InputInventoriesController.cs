using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
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
    }
}
