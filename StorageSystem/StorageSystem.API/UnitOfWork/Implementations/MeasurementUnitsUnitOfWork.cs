using StorageSystem.API.Repositories.Implementations;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class MeasurementUnitsUnitOfWork : GenericUnitOfWork<MeasurementUnit>, IMeasurementUnitsUnitOfWork
    {
        private readonly IMeasurementUnitsRepository _measurementUnitsRepository;

        public MeasurementUnitsUnitOfWork(IGenericRepository<MeasurementUnit> repository, IMeasurementUnitsRepository measurementUnitsRepository): base(repository)
        {
            _measurementUnitsRepository = measurementUnitsRepository;
        }

        public async Task<IEnumerable<MeasurementUnit>> GetComboAsync() => await _measurementUnitsRepository.GetComboAsync();

        public async Task<ActionResponse<IEnumerable<MeasurementUnit>>> GetAsync(PaginationDTO pagination) => await _measurementUnitsRepository.GetAsync(pagination);        

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _measurementUnitsRepository.GetTotalPagesAsync(pagination);

        public async Task<ActionResponse<double>> GetBaseUnitWithFactor(string physycalState, int meausementUnitFactorId) => await _measurementUnitsRepository.GetBaseUnitWithFactor(physycalState, meausementUnitFactorId);

        public async Task<ActionResponse<int>> GetMeasurementUnitById(string MCode, string MName) => await _measurementUnitsRepository.GetMeasurementUnitById(MCode, MName);
    }
}
