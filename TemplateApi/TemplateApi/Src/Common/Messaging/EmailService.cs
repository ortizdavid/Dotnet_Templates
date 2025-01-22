using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace TemplateApi.Common.Messaging
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public void SendEmail(string? recipientEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(recipientEmail))
            {
                throw new ArgumentException("Recipient email address cannot be null or empty.", nameof(recipientEmail));
            }

            if (string.IsNullOrEmpty(_settings.Host) || string.IsNullOrEmpty(_settings.User) || string.IsNullOrEmpty(_settings.Password))
            {
                throw new InvalidOperationException("SMTP settings are not fully configured.");
            }

            try
            {
                using var smtpClient = new SmtpClient(_settings.Host)
                {
                    Port = _settings.Port ?? 587,
                    Credentials = new NetworkCredential(_settings.User, _settings.Password),
                    EnableSsl = _settings.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.User),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error sending email", ex);
            }
        }
    }
}