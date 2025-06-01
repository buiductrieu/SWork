using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWork.Data.DTO.CVDTO;
using SWork.ServiceContract.Interfaces;

namespace SWork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeTemplatesController: Controller
    {
        private readonly ITemplateResumeService _resumeTemplateService;
        private readonly IMapper _mapper;

        public ResumeTemplatesController(ITemplateResumeService resumeTemplateService, IMapper mapper)
        {
            _resumeTemplateService = resumeTemplateService;
            _mapper = mapper;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TemplateResumeDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _resumeTemplateService.CreateTResumeAsync(model);
                return Ok("Template was updated succesfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _resumeTemplateService.GetTResumeByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [HttpGet("get-pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _resumeTemplateService.GetPaginatedTResumeAsync(pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string nameTemplate, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _resumeTemplateService.SearchTResumeAsync(nameTemplate, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TemplateResumeDTO model)
        {

            var existing = await _resumeTemplateService.GetTResumeByIdAsync(id);
            if (existing == null)
                return NotFound("Template not found");

            _mapper.Map(model, existing);
            await _resumeTemplateService.UpdateTResumeAsync(existing);
            return Ok("Template was updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _resumeTemplateService.DeleteTResumeAsync(id);
                return Ok("Template was deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }
}