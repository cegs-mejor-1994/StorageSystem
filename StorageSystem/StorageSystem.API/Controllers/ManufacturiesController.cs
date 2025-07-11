using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManufacturiesController : GenericController<Manufactury>
    {
        private readonly IManufacturiesUnitOfWork _manufacturiesUnitOfWork;

        public ManufacturiesController(IGenericUnitOfWork<Manufactury> unitOfWork, IManufacturiesUnitOfWork manufacturiesUnitOfWork) : base(unitOfWork)
        {
            _manufacturiesUnitOfWork = manufacturiesUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _manufacturiesUnitOfWork.GetComboAsync());
        }
    }
}
