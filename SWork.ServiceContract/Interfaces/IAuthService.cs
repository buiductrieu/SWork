﻿

namespace SWork.ServiceContract.Interfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser> RegisterAsync(UserRegisterDTO dto);
        Task<bool> ConfirmEmail(string email, string token);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task LogoutAsync(string refreshToken);
    }
}
