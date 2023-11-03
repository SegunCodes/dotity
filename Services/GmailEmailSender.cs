using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace dotity.Services
{
    public class GmailEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public GmailEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Load SMTP settings from configuration
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];
            var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

            using (var client = new SmtpClient(host))
            {
                client.Port = port;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = enableSsl;

                var message = new MailMessage
                {
                    From = new MailAddress(username),
                    Subject = subject,
                    Body = htmlMessage
                };

                message.To.Add(email);

                await client.SendMailAsync(message);
            }
        }
    }
}
