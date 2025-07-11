using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface ITypeReferenceProductUnitOfWork
    {                
        Task<IEnumerable<TypeReferenceProduct>> GetComboAsync();
        Task<ActionResponse<IEnumerable<TypeReferenceProduct>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);         
    }
}
