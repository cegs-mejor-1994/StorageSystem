using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RawMaterialsController : GenericController<RawMaterial>
    {        
        private readonly IRawMaterialsUnitOfWork _rawMaterialsUnitOfWork;

        public RawMaterialsController(IGenericUnitOfWork<RawMaterial> unitOfWork, IRawMaterialsUnitOfWork rawMaterialsUnitOfWork) : base(unitOfWork)
        {
            _rawMaterialsUnitOfWork = rawMaterialsUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _rawMaterialsUnitOfWork.GetComboAsync());
        }
    }
}
