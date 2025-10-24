using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : GenericController<Supplier>
    {
        private readonly ISuppliersUnitOfWork _suppliersUnitOfWork;

        public SuppliersController(IGenericUnitOfWork<Supplier> unitOfWork, ISuppliersUnitOfWork suppliersUnitOfWork) : base(unitOfWork)
        {            
            _suppliersUnitOfWork = suppliersUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _suppliersUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _suppliersUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _suppliersUnitOfWork.GetComboAsync());
        }

        [HttpGet("getSupplierById")]
        public async Task<IActionResult> GetCategoryByID(string SNit)
        {
            var action = await _suppliersUnitOfWork.GetSupplierById(SNit);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
