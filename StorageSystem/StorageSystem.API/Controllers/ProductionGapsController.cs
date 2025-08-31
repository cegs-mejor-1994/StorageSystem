using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionGapsController : GenericController<ProductionGap>
    {
        private readonly IProductionGapsUnitOfWork _productionGapsUnitOfWork;

        public ProductionGapsController(IGenericUnitOfWork<ProductionGap> unitOfWork, IProductionGapsUnitOfWork productionGapsUnitOfWork) : base(unitOfWork)
        {
            _productionGapsUnitOfWork = productionGapsUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _productionGapsUnitOfWork.GetComboAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _productionGapsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _productionGapsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
