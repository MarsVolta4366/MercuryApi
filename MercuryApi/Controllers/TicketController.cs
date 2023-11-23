using MercuryApi.BLL;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Mvc;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketBusinessLogic _ticketBusinessLogic;

        public TicketController(ITicketBusinessLogic ticketBusinessLogic)
        {
            _ticketBusinessLogic = ticketBusinessLogic;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateTicket([FromBody] TicketUpsert request)
        {
            TicketDto response = await _ticketBusinessLogic.CreateTicket(request);

            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateTicket([FromBody] TicketUpsert request)
        {
            TicketDto? response = await _ticketBusinessLogic.UpdateTicket(request);

            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
