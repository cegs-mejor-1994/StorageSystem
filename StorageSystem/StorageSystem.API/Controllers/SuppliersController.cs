using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
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

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _suppliersUnitOfWork.GetComboAsync());
        }
    }
}
