using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Producer.API.Data;
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
        private readonly ProducerDBContext _producerDBContext;
        public MessagesController(ILogger<MessagesController> logger, IMessageProducer messageProducer, ProducerDBContext producerDBContext)
        {
            _logger = logger;
            _messageProducer = messageProducer;
            _producerDBContext = producerDBContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Message message)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _producerDBContext.Messages.AddAsync(message);
            await _producerDBContext.SaveChangesAsync();

            await _messageProducer.SendMesage(message);

            return Ok(message);
        }


    }
}
