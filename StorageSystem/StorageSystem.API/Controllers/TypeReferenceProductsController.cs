using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
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

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _typeReferenceProductUnitOfWork.GetComboAsync());
        }
    }
}
