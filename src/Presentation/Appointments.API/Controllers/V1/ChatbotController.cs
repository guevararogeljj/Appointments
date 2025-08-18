using Appointments.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("train")]
        public IActionResult Train([FromBody] string dataPath)
        {
            _chatbotService.Train(dataPath);
            return Ok("Modelo entrenado correctamente");
        }

        [HttpPost("ask")]
        public IActionResult Ask([FromBody] string pregunta)
        {
            var respuesta = _chatbotService.GetAnswer(pregunta);
            return Ok(new { respuesta });
        }
    }
}

