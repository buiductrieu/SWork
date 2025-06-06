


namespace SWork.ServiceContract.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(EmailRequestDTO request);
        public void SendEmailConfirmation(string username, string confirmLink);
        Task SendEmailAsync(ApplicationUser user, string resetLink);

    }
}
