using MercuryApi.BLL;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.NonEntityDtos;
using MercuryApi.Data.RequestModels;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamBusinessLogic _teamBusinessLogic;

        public TeamController(ITeamBusinessLogic teamBusinessLogic)
        {
            _teamBusinessLogic = teamBusinessLogic;
        }

        [HttpGet("get-current-users-teams")]
        public async Task<ActionResult> GetCurrentUsersTeams()
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<TeamDto> response = await _teamBusinessLogic.GetUsersTeams(int.Parse(userId));
            return Ok(response);
        }

        [HttpGet("get-team-by-id/{teamId}")]
        public async Task<ActionResult> GetTeamById([FromRoute] int teamId)
        {
            TeamDto? response = await _teamBusinessLogic.GetTeamById(teamId);
            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpGet("check-if-team-name-exists/{teamName}")]
        public async Task<ActionResult> CheckIfTeamNameExists([FromRoute] string teamName)
        {
            ExistsDto response = await _teamBusinessLogic.TeamNameExists(teamName);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateTeam([FromBody] TeamUpsert request)
        {
            // Get the id of the user who's creating the team and add them to the team.
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            TeamDto? response = await _teamBusinessLogic.CreateTeam(request, int.Parse(userId));
            if (response == null) return BadRequest($"Team name {request.Name} is already taken.");
            return Ok(response);
        }

        [HttpPost("add-users-to-team")]
        public async Task<ActionResult> AddUsersToTeam([FromBody] TeamUpsert request)
        {
            TeamDto? response = await _teamBusinessLogic.AddUsersToTeam(request);
            if (response == null) return NotFound("Team not found.");

            return Ok(response);
        }

        [HttpPost("remove-user-from-team")]
        public async Task<ActionResult> RemoveUserFromTeam([FromBody] RemoveUserFromTeamRequest request)
        {
            TeamDto? response = await _teamBusinessLogic.RemoveUserFromTeam(request);
            if (response == null) return NotFound("Team not found.");
            return Ok(response);
        }

        [HttpGet("user-is-in-team/{teamId}")]
        public async Task<ActionResult> UserIsInTeam([FromRoute] int teamId)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            TeamDto? response = await _teamBusinessLogic.UserIsInTeam(int.Parse(userId), teamId);
            if (response == null) return Unauthorized();

            return Ok(response);
        }

        [HttpDelete("delete-team-by-id/{teamId}")]
        public async Task<ActionResult> DeleteTeamById([FromRoute] int teamId)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _teamBusinessLogic.DeleteTeamById(teamId, int.Parse(userId));

            return NoContent();
        }
    }
}
