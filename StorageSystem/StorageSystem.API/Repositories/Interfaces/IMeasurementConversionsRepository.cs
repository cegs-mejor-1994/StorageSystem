using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IMeasurementConversionsRepository
    {
        Task<IEnumerable<MeasurementConversion>> GetComboAsync();
    }
}
