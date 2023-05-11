using EmailSender.Producer;
using EmailSender.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailSender.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSendingController : ControllerBase
    {
        private readonly IEmailSendingService _emailSendingService;

        // POST api/<EmailSendingController>
        [HttpPost]
        public void Post([FromBody] Email email)
        {
            _emailSendingService.SendEmail(email);
        }

    }
}
