using EmailSender.Producer;

namespace EmailSender.WebApi.Services
{
    public interface IEmailSendingService
    {
        public void SendEmail(Email email);
    }
}
