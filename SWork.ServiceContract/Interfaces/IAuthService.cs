using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWork.Data.DTO.AuthDTO;
using SWork.Data.DTO.UserDTO;
using SWork.Data.Entities;

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
