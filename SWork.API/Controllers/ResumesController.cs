using Microsoft.AspNetCore.Mvc;
using SWork.Data.DTO.CVDTO;
using SWork.Service.Services;
using SWork.ServiceContract.Interfaces;

namespace SWork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : Controller
    {
        private readonly IResumeService _resumeService;

        public ResumesController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateResumeDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _resumeService.CreateResumeAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateResumeDTO model)
        {
            var existing = await _resumeService.GetResumeByIdAsync(id);
            if (existing == null)
                return NotFound("Category not found");

            await _resumeService.UpdateResumeAsync(existing);
            return Ok("Resume updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _resumeService.DeleteResumeAsync(id);
                return Ok("Resume was deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var resume = await _resumeService.GetResumeByIdAsync(id);
                if (resume == null) return NotFound();
                return Ok(resume);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? nameResume, [FromQuery] int? studentId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _resumeService.SearchResumeAsync(nameResume, studentId, pageIndex, pageSize);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var results = await _resumeService.GetPaginatedResumeAsync(pageIndex, pageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
