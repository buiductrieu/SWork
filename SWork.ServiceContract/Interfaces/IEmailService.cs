using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWork.Data.DTO.AuthDTO;
using SWork.Data.Entities;

namespace SWork.ServiceContract.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(EmailRequestDTO request);
        public void SendEmailConfirmation(string username, string confirmLink);
        Task SendEmailAsync(ApplicationUser user, string resetLink);

    }
}
