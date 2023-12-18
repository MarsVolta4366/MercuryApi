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
    public class SprintController : ControllerBase
    {
        private readonly ISprintBusinessLogic _sprintBusinessLogic;

        public SprintController(ISprintBusinessLogic sprintBusinessLogic)
        {
            _sprintBusinessLogic = sprintBusinessLogic;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSprint([FromBody] SprintUpsert request)
        {
            SprintDto response = await _sprintBusinessLogic.CreateSprint(request);
            return Ok(response);
        }

        [HttpDelete("{sprintId}")]
        public async Task<ActionResult> DeleteSprint([FromRoute] int sprintId)
        {
            await _sprintBusinessLogic.DeleteSprint(sprintId);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSprint([FromBody] SprintUpsert request)
        {
            SprintDto? response = await _sprintBusinessLogic.UpdateSprint(request);

            return response != null ? Ok(response) : NotFound();
        }
    }
}
