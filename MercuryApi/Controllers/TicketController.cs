using MercuryApi.BLL;
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
            await _ticketBusinessLogic.CreateTicket(request);

            return Ok("Ticket created.");
        }
    }
}
