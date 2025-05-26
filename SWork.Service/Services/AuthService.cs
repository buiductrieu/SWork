using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWork.Data.DTO;
using SWork.Data.Entities;
using SWork.RepositoryContract.IUnitOfWork;
using SWork.ServiceContract.Interfaces;

namespace SWork.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleManager = roleManager;
            
        }

        public async Task<ApplicationUser> RegisterAsync(UserRegisterDTO dto)
        {
            var phoneNumberExisted = await _unitOfWork.UserRepository.PhoneNumberExistsAsync(dto.PhoneNumber);
            var userExisted = await _unitOfWork.UserRepository.UsernameExistsAsync(dto.UserName);

            if (userExisted)
            {
                throw new BadHttpRequestException("Tên người dùng đã tồn tại");
            }

            if (phoneNumberExisted)
            {
                throw new BadHttpRequestException("Số điện thoại đã tồn tại");
            }

            var user = _mapper.Map<ApplicationUser>(dto);
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Không tạo được người dùng: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (dto.Role == "Employer" || dto.Role == "Admin" || dto.Role == "Student")
            {
                if (!await _roleManager.RoleExistsAsync(dto.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(dto.Role));
                }

                await _userManager.AddToRoleAsync(user, dto.Role);
            }
            else
            {
                // Tìm người dùng bằng tên người dùng (không phải email)
                var userToDelete = await _userManager.FindByNameAsync(dto.UserName);
                if (userToDelete != null)
                {
                    await _userManager.DeleteAsync(userToDelete);
                }
                throw new BadHttpRequestException("Vai trò không hợp lệ");
            }

            await _unitOfWork.SaveChangeAsync();

            return user;
        }

        public async Task<bool> ConfirmEmail(string username, string token)
        {
            var user = await _userManager.FindByEmailAsync(username) ?? throw (new BadHttpRequestException("User not found"));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(ApplicationUser user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                Expries = DateTime.UtcNow.AddDays(7),//Refresh hết hạn sau 7 ngày
                Created = DateTime.UtcNow
            };
            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangeAsync();
            return refreshToken;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), //Thông tin chủ thể của object: tên đăng nhập của user
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),// unique identifier giúp phân biệt các token khác nhau, Sử dụng NewGuid() để tạo ra một giá trị đi nhất
                new Claim(ClaimTypes.NameIdentifier, user.Id), //Id để xác định người dùng 1 cách duy nhất 
                new Claim(ClaimTypes.Email, user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user == null)
                throw new BadHttpRequestException("Username or password is incorrect!");

            var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (!isValid)
                throw new BadHttpRequestException("Username or password is incorrect!");

            //if (!user.IsActive)
            //    throw new BadHttpRequestException("Your account is banned!");

            //if (!user.EmailConfirmed)
            //    return null;


            var token = await GenerateJwtToken(user);
            var refreshToken = await GetRefreshTokenAsync(user);
            var userDTO = _mapper.Map<UserDTO>(user);

            var role = await _userManager.GetRolesAsync(user);

            LoginResponseDTO responseDTO = new()
            {
                User = userDTO,
                Token = token,
                RefreshToken = refreshToken.Token,
                Role = role
            };

            return responseDTO;
        }


    }
}
