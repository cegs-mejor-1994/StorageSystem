using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeReferenceProductsController : GenericController<TypeReferenceProduct>
    {
        private readonly ITypeReferenceProductUnitOfWork _typeReferenceProductUnitOfWork;

        public TypeReferenceProductsController(IGenericUnitOfWork<TypeReferenceProduct> unitOfWork, ITypeReferenceProductUnitOfWork typeReferenceProductUnitOfWork) : base(unitOfWork)
        {
            _typeReferenceProductUnitOfWork = typeReferenceProductUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _typeReferenceProductUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _typeReferenceProductUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _typeReferenceProductUnitOfWork.GetComboAsync());
        }
    }
}
