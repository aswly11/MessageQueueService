using Consumer.API.Data;
using Consumer.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consumer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private  ConsumerDBContext _consumerDBContext;
        public MessagesController(ILogger<MessagesController> logger, ConsumerDBContext consumerDBContext)
        {
            _logger = logger;
            _consumerDBContext = consumerDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<Message>> Get()
        {
            var messages = await _consumerDBContext.Messages.ToListAsync();
            return Ok(messages);
        }
    }
}
