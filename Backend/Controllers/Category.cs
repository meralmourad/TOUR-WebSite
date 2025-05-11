using Backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Category : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        public Category(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.category.GetAllAsync();
            var result = categories.Select(c => new 
            {
                c.Id,
                c.Name
            });
            return Ok(result);
        }
    }
}
