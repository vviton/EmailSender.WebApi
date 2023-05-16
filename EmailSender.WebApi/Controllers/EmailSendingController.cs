using EmailSender.Producer;
using EmailSender.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailSender.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSendingController : ControllerBase
    {
        ///private readonly IEmailSendingService _emailSendingService;
        private readonly IRabbitMqProducer<Email> _rabbitMqProducer; //= new EmailProducer("localhost",5672);
        // POST api/<EmailSendingController>
        public EmailSendingController(IRabbitMqProducer<Email> producer)
        {
            _rabbitMqProducer = producer;
        }
        [HttpPost]
        public void Post([FromBody] Email email)
        {
            _rabbitMqProducer.SendToQueue(email);
        }

    }
}
