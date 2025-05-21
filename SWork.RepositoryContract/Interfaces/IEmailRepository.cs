using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWork.Data.DTO;

namespace SWork.RepositoryContract.Interfaces
{
    public interface IEmailRepository
    {
        public void SendEmail(EmailRequestDTO request);
        public void SendEmailConfirmation(EmailRequestDTO request, string confirmLink);
        void SendEmailForgotPassword(EmailRequestDTO request, string resetLink);

    }
}
