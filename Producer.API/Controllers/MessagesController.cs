using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Producer.API.Models;
using Producer.API.Services;

namespace Producer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageProducer _messageProducer;

        public static readonly List<Message> messages = new();
        public MessagesController(ILogger<MessagesController> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Message message)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            messages.Add(message);
            await _messageProducer.SendMesage(message);

            return Ok(message);
        }


    }
}
