using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
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

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _manufacturiesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _manufacturiesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
