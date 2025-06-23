using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using System.Security.Cryptography.Xml;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferencesController : GenericController<Reference>
    {
        private readonly IReferencesUnitOfWork _referencesUnitOfWork;

        public ReferencesController(IGenericUnitOfWork<Reference> unitOfWork, IReferencesUnitOfWork referencesUnitOfWork) : base(unitOfWork)
        {
            _referencesUnitOfWork = referencesUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _referencesUnitOfWork.GetComboAsync());
        }
    }
}
