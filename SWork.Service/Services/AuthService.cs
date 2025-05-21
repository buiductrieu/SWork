using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SWork.Data.DTO;
using SWork.Data.Entities;
using SWork.RepositoryContract.Interfaces;
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

            await _unitOfWork.SaveChanges();

            return user;
        }

        public async Task<bool> ConfirmEmail(string username, string token)
        {
            var user = await _userManager.FindByEmailAsync(username) ?? throw (new BadHttpRequestException("User not found"));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
    }
}
