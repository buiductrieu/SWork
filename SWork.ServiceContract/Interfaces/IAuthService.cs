using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWork.Data.DTO;
using SWork.Data.Entities;

namespace SWork.ServiceContract.Interfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser> RegisterAsync(UserRegisterDTO dto);
        Task<bool> ConfirmEmail(string email, string token);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
    }
}
