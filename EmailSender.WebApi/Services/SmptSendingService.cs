using EmailSender.Producer;
using System.Net.Mail;

namespace EmailSender.WebApi.Services
{
    public class SmptSendingService : IEmailSendingService
    {
        private SmtpClient _smptClient;

        public void SendEmail(Email email)
        {
            
        }
    }
}
