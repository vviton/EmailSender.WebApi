using EmailSender.Producer;
using EmailSender.WebApi.Services;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EmailSender.WebApi.RabbitMQ
{
    public class EmailConsumer
    {
        private readonly ConnectionFactory _factory;
        private readonly IEmailSendingService _emailSendingService;
       

        public EmailConsumer(string hostName, int portNumber, IEmailSendingService emailSendingService)
        {
            _factory = new ConnectionFactory() { HostName = hostName, Port = portNumber };
            _emailSendingService = emailSendingService;
            StartConsuming();
        }

        public void StartConsuming()
        {

            using (var connection = _factory.CreateConnection())
            {
                using (var emailChannel = connection.CreateModel())
                {
                    emailChannel.QueueDeclare(
                        queue: "email_message",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var consumer = new EventingBasicConsumer(emailChannel);
                    consumer.Received += (model, eventArgs) =>
                    {
                        var body = eventArgs.Body.ToArray();
                        var msg = Encoding.UTF8.GetString(body);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            Email email = JsonConvert.DeserializeObject<Email>(msg);
                            _emailSendingService.SendEmail(email);
                        }
                        emailChannel.BasicAck(eventArgs.DeliveryTag,false);
                    };
                    
                }
            }
        }
    }
}
