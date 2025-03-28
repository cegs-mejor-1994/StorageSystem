using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using StorageSystem.API.UnitOfWork.Interfaces;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RawMaterialsController : GenericController<RawMaterial>
    {
        private readonly IGenericUnitOfWork<RawMaterial> _unitOfWork;

        public RawMaterialsController(IGenericUnitOfWork<RawMaterial> unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
