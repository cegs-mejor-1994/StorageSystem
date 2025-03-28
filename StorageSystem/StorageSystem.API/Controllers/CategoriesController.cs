using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using StorageSystem.API.UnitOfWork.Interfaces;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : GenericController<Category>
    {
        private readonly IGenericUnitOfWork<Category> _unitOfWork;

        public CategoriesController(IGenericUnitOfWork<Category> unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
