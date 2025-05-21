using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SWork.Common.Helper;
using SWork.Data.DTO;
using SWork.Data.Entities;
using SWork.ServiceContract.Interfaces;
using AutoMapper;

namespace SWork.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("default")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager, IEmailService emailService, IMapper mapper)
        {
            _authService = authService;
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> Register(UserRegisterDTO dto)
        {
            var response = new APIResponse();
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Result = ModelState;
                return BadRequest(response);
            }

            var result = await _authService.RegisterAsync(dto);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(result);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token = token, username = result.UserName }, Request.Scheme);

            _emailService.SendEmailConfirmation(result.Email, confirmationLink);

            // Map to UserResponseDTO
            var userResponse = _mapper.Map<UserResponseDTO>(result);

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = userResponse;

            return Ok(response);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> ConfirmEmail([FromQuery] string token, [FromQuery] string username)
        {
            var response = new APIResponse();
            try
            {
                var result = await _authService.ConfirmEmail(username, token);
                if (result)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                    return Ok(response);
                }
                response.IsSuccess = false;
                response.ErrorMessages.Add("Can not confirm your email");
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages.Add(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
        }
    }
}