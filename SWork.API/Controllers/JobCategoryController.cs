using Microsoft.AspNetCore.Mvc;
using SWork.Data.DTO;
using SWork.ServiceContract.Interfaces;

namespace SWork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCategoryController : Controller
    {
        private readonly IJobCategoryService _jobCategoryService;

        public JobCategoryController(IJobCategoryService jobCategoryService)
        {
            _jobCategoryService = jobCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _jobCategoryService.GetAllJobCategoryAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _jobCategoryService.GetCategoryByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _jobCategoryService.GetCategoryByName(name);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobCategoryDTO dto)
        {
            await _jobCategoryService.CreateJobCategoryAsync(dto);
            return Ok("Category created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobCategoryDTO dto)
        {
            var existing = await _jobCategoryService.GetCategoryByIdAsync(id);
            if (existing == null)
                return NotFound("Category not found");

            // Map manually or use AutoMapper before update
            existing.CategoryName = dto.CategoryName;
            existing.Description = dto.Description;

            await _jobCategoryService.UpdateJobCategoryAsync(existing);
            return Ok("Category updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _jobCategoryService.DeleteJobCategoryAsync(id);
            return Ok("Category deleted successfully.");
        }
    }
}
