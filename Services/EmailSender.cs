using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace dotity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine("Email address: " + email);
            try
            {
                // Load SMTP settings from configuration
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var host = smtpSettings["Host"];
                var port = int.Parse(smtpSettings["Port"]);
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];
                var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                // using (var client = new SmtpClient(host))
                // {
                //     client.Port = port;
                //     client.UseDefaultCredentials = false;
                //     client.Credentials = new NetworkCredential(username, password);
                //     client.EnableSsl = enableSsl;

                //     using var message = new MailMessage
                //     {
                //         From = new MailAddress(username),
                //         Subject = subject,
                //         Body = htmlMessage
                //     };
                //     message.To.Add(email);

                //     await client.SendMailAsync(message);
                // }
                using (var message = new MailMessage
                {
                    From = new MailAddress(username),
                    Subject = subject,
                    Body = htmlMessage
                })
                {
                    message.To.Add(email);

                    using (var client = new SmtpClient(host))
                    {
                        client.Port = port;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(username, password);
                        client.EnableSsl = enableSsl;

                        await client.SendMailAsync(message);
                    }
                }

            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; // Rethrow the exception to signal the failure
            }
        }
    }
}
