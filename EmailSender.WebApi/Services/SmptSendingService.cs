using EmailSender.Producer;
using EmailSender.WebApi.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailSender.WebApi.Services
{
    public class SmptSendingService : IEmailSendingService
    {
        private SmtpClient _smtpClient;
        private readonly EmailSettings _emailSettings;


        public SmptSendingService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
            ConfigureSmptClient();
        }

        public void SendEmail(Email email)
        {
            MailAddress from = new (email.From);
            MailAddress to = new (email.To);
            MailMessage mailMessage = new MailMessage(from,to);
            mailMessage.Bcc.Add(email.Bcc);
            mailMessage.CC.Add(email.Cc);
            mailMessage.Body = email.Body;
            mailMessage.Subject = email.Subject;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            _smtpClient.SendMailAsync(mailMessage);
        }
        private void ConfigureSmptClient()
        {
            string server = _emailSettings.SmtpServer;
            string pass = _emailSettings.Password;
            int port = _emailSettings.SmtpPort;
            var smtpClient = new SmtpClient
            {
                Host = _emailSettings.SmtpServer,
                Port = _emailSettings.SmtpPort,
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password)
            };
            this._smtpClient = smtpClient;
        }
    }
}
