using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWork.Data.DTO.JobBookMarkDTO;
using SWork.Data.DTO.JobDTO;
using SWork.Service.Services;
using SWork.ServiceContract.Interfaces;
using System.Security.Claims;

namespace SWork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobBookMarkController : Controller
    {
        private readonly IJobBookMarkService _markService;
        private readonly IJobService _jobSevice;

        public JobBookMarkController(IJobBookMarkService markService, IJobService jobService)
        {
            _jobSevice = jobService;
            _markService = markService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateBookMark([FromForm] MarkDTO dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                await _markService.AddJobBookMark(userId, dto);
                return Ok("Đã thêm vào mục công việc yêu thích.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("student/bookmarked-jobs")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetBookmarkedJobs([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Lấy userId từ token
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _jobSevice.GetJobMarkByIdAsync(userId, pageIndex, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("delete")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteJob(int markId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                await _markService.RemoveJobBookMark(userId, markId);
                return Ok("Xóa công việc yêu thích thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
