using SWork.Data.DTO.AuthDTO;

namespace SWork.RepositoryContract.Interfaces
{
    public interface IEmailRepository
    {
        public void SendEmail(EmailRequestDTO request);
        public void SendEmailConfirmation(EmailRequestDTO request, string confirmLink);
        void SendEmailForgotPassword(EmailRequestDTO request, string resetLink);

    }
}
