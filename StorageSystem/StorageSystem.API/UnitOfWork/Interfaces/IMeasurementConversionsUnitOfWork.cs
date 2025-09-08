using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IMeasurementConversionsUnitOfWork
    {
        Task<IEnumerable<MeasurementConversion>> GetComboAsync();
    }
}
