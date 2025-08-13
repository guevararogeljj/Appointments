using Appointments.Application.Features.Messaging.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Appointments.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MessagingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessagingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}

