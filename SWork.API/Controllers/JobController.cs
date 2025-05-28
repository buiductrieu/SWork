using Microsoft.AspNetCore.Mvc;
using SWork.Data.DTO.JobDTO;
using SWork.ServiceContract.Interfaces;

namespace SWork.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }


        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginatedJobs(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var jobs = await _jobService.GetPaginatedJobAsync(pageIndex, pageSize);
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetJobById(int id)
        {
            try
            {
                await _jobService.GetJobByIdAsync(id);
                return Ok("Job have exsit.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateJob([FromForm] CreateJobDTO dto)
        {
            try
            {
                await _jobService.CreateJobAsync(dto);
                return Ok("Job created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateJob(int id, [FromForm] UpdateJobDTO dto)
        {
            try
            {
                var job = await _jobService.GetJobByIdAsync(id);
                if (job == null) return NotFound("Job not found.");

                // Cập nhật chỉ các trường có giá trị khác null
                if (dto.Title != null) job.Title = dto.Title;
                if (dto.Description != null) job.Description = dto.Description;
                if (dto.Requirements != null) job.Requirements = dto.Requirements;
                if (dto.Location != null) job.Location = dto.Location;
                if (dto.Salary.HasValue) job.Salary = dto.Salary.Value;
                if (dto.Status != null) job.Status = dto.Status;
                if (dto.WorkingHours != null) job.WorkingHours = dto.WorkingHours;
                if (dto.StartDate.HasValue) job.StartDate = dto.StartDate.Value;
                if (dto.EndDate.HasValue) job.EndDate = dto.EndDate.Value;
                if (dto.EmployerID.HasValue) job.EmployerID = dto.EmployerID.Value;
                if (dto.SubscriptionID.HasValue) job.SubscriptionID = dto.SubscriptionID.Value;
                if (dto.CategoryID.HasValue) job.CategoryID = dto.CategoryID.Value;

                await _jobService.UpdateJobAsync(job, dto.Image);

                return Ok("Job updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                await _jobService.DeleteJobAsync(id);
                return Ok("Job deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
