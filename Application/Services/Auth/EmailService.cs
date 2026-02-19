using Application.Serices.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Auth
{
    public class EmailService(IConfiguration _configuration) : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = _configuration["EmailSettings:Email"];
            var password = _configuration["EmailSettings:Password"];
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);

            var smtpClient = new System.Net.Mail.SmtpClient(host, port)
            {
                Port = port,
                Credentials = new System.Net.NetworkCredential(email, password),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
