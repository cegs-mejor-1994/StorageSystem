using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementUnitsController : GenericController<MeasurementUnit>
    {
        private readonly IMeasurementUnitsUnitOfWork _measurementUnitsUnitOfWork;

        public MeasurementUnitsController(IGenericUnitOfWork<MeasurementUnit> unitOfWork, IMeasurementUnitsUnitOfWork measurementUnitsUnitOfWork) : base(unitOfWork)
        {
            _measurementUnitsUnitOfWork = measurementUnitsUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _measurementUnitsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _measurementUnitsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _measurementUnitsUnitOfWork.GetComboAsync());
        }

        [HttpGet("baseUnitWithFactor")]
        public async Task<IActionResult> GetBaseUnitWithFactor(string physycalState, int meausementUnitFactorId)
        {
            var action = await _measurementUnitsUnitOfWork.GetBaseUnitWithFactor(physycalState, meausementUnitFactorId);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
