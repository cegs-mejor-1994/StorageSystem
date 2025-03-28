using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : GenericController<Supplier>
    {
        private readonly IGenericUnitOfWork<Supplier> _unitOfWork;

        public SuppliersController(IGenericUnitOfWork<Supplier> unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
