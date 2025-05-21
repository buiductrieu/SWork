﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SWork.Data.DTO;
using SWork.Data.Entities;
using SWork.RepositoryContract.Interfaces;
using SWork.ServiceContract.Interfaces;

namespace SWork.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IConfiguration _configuration;

        public EmailService(IEmailRepository emailRepository, IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;
        }
        public void SendEmail(EmailRequestDTO request)
        {
            try
            {
                _emailRepository.SendEmail(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public void SendEmailConfirmation(string email, string confirmLink)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email address cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(confirmLink))
            {
                throw new ArgumentException("Confirmation link cannot be empty");
            }

            try
            {
                EmailRequestDTO request = new EmailRequestDTO
                {
                    Subject = "SWork Email Confirmation",
                    Body = "",
                    toEmail = email
                };
                _emailRepository.SendEmailConfirmation(request, confirmLink);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send confirmation email: {ex.Message}", ex);
            }
        }

        public async Task SendEmailAsync(ApplicationUser user, string resetLink)
        {
            var emailRequest = new EmailRequestDTO
            {
                Subject = "Password Reset Request",
                toEmail = user.Email // Sử dụng email thực của người dùng
            };

            _emailRepository.SendEmailForgotPassword(emailRequest, resetLink);

        }

    }
}
