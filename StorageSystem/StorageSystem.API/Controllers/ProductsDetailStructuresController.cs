using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;


namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsDetailStructuresController : GenericController<ProductsDetailStructure>
    {
        private readonly IProductsDetailStructuresUnitOfWork _productsDetailStructuresUnitOfWork;

        public ProductsDetailStructuresController(IGenericUnitOfWork<ProductsDetailStructure> unitOfWork, IProductsDetailStructuresUnitOfWork productsDetailStructuresUnitOfWork) : base(unitOfWork)    
        {
            _productsDetailStructuresUnitOfWork = productsDetailStructuresUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync(int productDetailID)
        {
            return Ok(await _productsDetailStructuresUnitOfWork.GetComboAsync(productDetailID));
        }
    }
}
