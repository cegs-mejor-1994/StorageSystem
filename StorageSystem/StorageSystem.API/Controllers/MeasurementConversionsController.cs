using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementConversionsController : GenericController<MeasurementConversion>
    {
        private readonly IMeasurementConversionsUnitOfWork _measurementConversionsUnitOfWork;

        public MeasurementConversionsController(IGenericUnitOfWork<MeasurementConversion> unitOfWork, IMeasurementConversionsUnitOfWork measurementConversionsUnitOfWork) : base(unitOfWork)
        {
            _measurementConversionsUnitOfWork = measurementConversionsUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _measurementConversionsUnitOfWork.GetComboAsync());
        }
    }
}
