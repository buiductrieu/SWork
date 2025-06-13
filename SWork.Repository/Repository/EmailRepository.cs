﻿
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SWork.Common.Helper;
using SWork.Data.DTO.AuthDTO;

namespace SWork.Repository.Repository
{
    public class EmailRepository : IEmailRepository
    {

        private readonly EmailSetting _emailSetting;
        private readonly ILogger<EmailRepository> _logger;
        public EmailRepository(IOptions<EmailSetting> options, ILogger<EmailRepository> logger)
        {
            _emailSetting = options.Value;
            _logger = logger;
        }


        public void SendEmail(EmailRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.toEmail))
            {
                throw new ArgumentException("Email address cannot be empty");
            }

            // Kiểm tra cấu hình email
            if (string.IsNullOrWhiteSpace(_emailSetting.Host))
            {
                throw new ArgumentException("SMTP host is not configured");
            }

            if (string.IsNullOrWhiteSpace(_emailSetting.Email))
            {
                throw new ArgumentException("Sender email is not configured");
            }

            if (string.IsNullOrWhiteSpace(_emailSetting.Password))
            {
                throw new ArgumentException("Email password is not configured");
            }

            // Kiểm tra định dạng email
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(request.toEmail);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email address format: " + request.toEmail);
            }

            var email = new MimeMessage();
            try
            {
                // Thêm From address với display name
                email.From.Add(new MailboxAddress(_emailSetting.DisplayName ?? "SWork", _emailSetting.Email));
                
                // Thêm To address
                email.To.Add(new MailboxAddress("", request.toEmail));
                email.Subject = request.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to send email to {Email}", request.toEmail);
                throw new Exception("Failed to send email: " + e.Message);
            }
        }

        public void SendEmailConfirmation(EmailRequestDTO request, string confirmLink)
        {
            var body = $"<h1>Email Confirmation</h1><p>Dear {request.toEmail},</p><p>Thank you for registering with us. Please confirm your email by clicking on the link below.</p><a href='{confirmLink}'>Click here to confirm your email</a>";
            request.Body = body;
            SendEmail(request);
        }


        public void SendEmailForgotPassword(EmailRequestDTO request, string resetLink)
        {
            string body = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Password Reset</title>
        <style>
            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
            .email-container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); }}
            .email-header {{ background-color: #5288DB; padding: 15px; text-align: center; color: #ffffff; border-radius: 10px 10px 0 0; }}
            .email-header h1 {{ margin: 0; font-size: 24px; }}
            .email-body {{ padding: 20px; font-size: 16px; line-height: 1.6; color: #333333; }}
            .email-body p {{ margin-bottom: 15px; }}
            .reset-button {{ display: inline-block; padding: 10px 20px; color: #ffffff; background-color: #5288DB; border-radius: 5px; text-decoration: none; font-size: 18px; }}
            .email-footer {{ text-align: center; padding: 10px; background-color: #f4f4f4; font-size: 12px; color: #777777; border-radius: 0 0 10px 10px; }}
            .email-footer a {{ color: #5288DB; text-decoration: none; }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                <h1>Password Reset Request</h1>
            </div>
            <div class='email-body'>
                <p>Dear {request.toEmail},</p>
                <p>We received a request to reset your password. Click the link below to set a new password. This link will expire soon for security reasons.</p>
                <p style=""text-align: center;"">
                    <a href=""{resetLink}"" class=""reset-button"" style=""color: white; font-weight: bold; text-decoration: none;"">Reset Password</a>
                </p>
                <p>If you did not request a password reset, please ignore this email. Your account remains secure.</p>
                <p>Best regards,</p>
                <p><strong>Cursus Team</strong></p>
            </div>
            <div class='email-footer'>
                <p>&copy; 2024 Cursus. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";

            request.Body = body;
            SendEmail(request);
        }

    }
}
