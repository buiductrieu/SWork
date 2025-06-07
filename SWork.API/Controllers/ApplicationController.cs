using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWork.Data.DTO.ApplicationDTO;
using SWork.ServiceContract.Interfaces;
using System.Security.Claims;

namespace SWork.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateApplication([FromForm] RequestApplyDTO request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _applicationService.CreateApplicationAsync(request, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer,Admin")]

        public async Task<IActionResult> UpdateApplication([FromForm] StatusOfApplyDTO request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _applicationService.UpdateApplicationAsync(request, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Employer,Admin,Student")]
        public async Task<IActionResult> GetApplicationById(int applyId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _applicationService.GetApplicationByIdAsync(applyId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("employer/applications")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> GetApplyRelatedJobForEmployer([FromQuery] int jobId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Lấy userId từ token đăng nhập
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Không thể xác thực người dùng." });
                }

                // Gọi service
                var result = await _applicationService.GetApplyRelatedJobForEmployer(userId, jobId, pageIndex, pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
