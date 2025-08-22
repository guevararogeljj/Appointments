using Appointments.Application.ML.Commands.KConsultores;
using Appointments.Application.ML.Commands.Reto97;
using Appointments.Application.ML.Queries.KConsultores;
using Appointments.Application.ML.Queries.Reto97;
using Appointments.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChatbotController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("train-kConsultores")]
        public async Task<IActionResult> Train([FromBody] KConsultoresCommand request)
        {
            await _mediator.Send(request);
            return Ok("Modelo entrenado correctamente");
        }

        [HttpPost("ask-KConsultores")]
        public async Task<IActionResult> AskKConsultores([FromBody] KPredictQuery request)
        {
            var response = await _mediator.Send(request);
            if (response == null)
            {
                return NotFound("No se encontró una respuesta para la pregunta.");
            }
            return Ok(new { reply = response.PredictedLabel });
        }
        
        ///Reto97 training
        [HttpPost("train-Reto97")]
        public async Task<IActionResult> TrainReto97([FromBody] Reto97Command request)
        {
            await _mediator.Send(request);
            return Ok("Modelo Reto97 entrenado correctamente");
        }
        
        [HttpPost("ask-Reto97")]
        public async Task<IActionResult> AskReto97([FromBody] Reto97PredictQuery request)
        {
            var response = await _mediator.Send(request);
            if (response == null)
            {
                return NotFound("No se encontró una respuesta para la pregunta.");
            }
            return Ok(new { reply = response.PredictedLabel });
        }
    }
}

