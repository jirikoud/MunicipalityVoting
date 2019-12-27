using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace VotingCoreWeb.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtpServer = _configuration.GetValue<string>("SmtpServer");
                var smtpSender = _configuration.GetValue<string>("SmtpSender");
                var smtpAccount = _configuration.GetValue<string>("SmtpAccount");
                var smtpPassword = _configuration.GetValue<string>("SmtpPassword");
                // Plug in your email service here to send an email.
                using (var client = new SmtpClient(smtpServer))
                {
                    if (!string.IsNullOrEmpty(smtpAccount))
                    {
                        client.Credentials = new NetworkCredential(smtpAccount, smtpPassword);
                    }
                    var addressFrom = new MailAddress(smtpSender);
                    var addressTo = new MailAddress(email);
                    var mailMessage = new MailMessage(addressFrom, addressTo)
                    {
                        Subject = subject,
                        SubjectEncoding = System.Text.Encoding.UTF8,
                        Body = message,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        IsBodyHtml = true,
                    };
                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error SendEmail({subject}, {email})");
            }
        }
    }
}
