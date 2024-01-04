using MercuryApi.BLL;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketBusinessLogic _ticketBusinessLogic;

        public TicketController(ITicketBusinessLogic ticketBusinessLogic)
        {
            _ticketBusinessLogic = ticketBusinessLogic;
        }

        [HttpGet("{ticketId}")]
        public async Task<ActionResult> GetTicketById([FromRoute] int ticketId)
        {
            TicketDto? response = await _ticketBusinessLogic.GetTicketById(ticketId);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket([FromBody] TicketUpsert request)
        {
            TicketDto response = await _ticketBusinessLogic.CreateTicket(request);

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTicket([FromBody] TicketUpsert request)
        {
            TicketDto? response = await _ticketBusinessLogic.UpdateTicket(request);

            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpDelete("{ticketId}")]
        public async Task<ActionResult> DeleteTicketById([FromRoute] int ticketId)
        {
            // TODO: Need stored pocedure or something to update ticket orders when a ticket gets deleted.
            await _ticketBusinessLogic.DeleteTicketById(ticketId);
            return NoContent();
        }

        [HttpPut("update-ticket-order")]
        public async Task<ActionResult> UpdateTicketOrder([FromBody] TicketOrderUpsert request)
        {
            TicketDto? response = await _ticketBusinessLogic.UpdateTicketOrder(request);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPut("update-ticket-order-and-sprint")]
        public async Task<ActionResult> UpdateTicketOrderAndSprint([FromBody] TicketOrderAndSprintUpsert request)
        {
            TicketDto? response = await _ticketBusinessLogic.UpdateTicketOrderAndSprint(request);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
