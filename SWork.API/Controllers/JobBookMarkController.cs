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

        public JobBookMarkController(IJobBookMarkService markService)
        {
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
