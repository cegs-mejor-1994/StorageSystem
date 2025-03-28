using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementUnitsController : GenericController<MeasurementUnit>
    {
        private readonly IGenericUnitOfWork<MeasurementUnit> _unitOfWork;

        public MeasurementUnitsController(IGenericUnitOfWork<MeasurementUnit> unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
