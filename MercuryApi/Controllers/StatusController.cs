using MercuryApi.BLL;
using MercuryApi.Data.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusBusinessLogic _statusBusinessLogic;

        public StatusController(IStatusBusinessLogic statusBusinessLogic)
        {
            _statusBusinessLogic = statusBusinessLogic;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStatuses()
        {
            IEnumerable<StatusDto> response = await _statusBusinessLogic.GetAllStatuses();
            return Ok(response);
        }
    }
}
