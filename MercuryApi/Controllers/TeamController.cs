using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.RequestModels;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public TeamController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet("get-current-users-teams"), Authorize]
        public async Task<ActionResult> GetCurrentUsersTeams()
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Team> usersTeams = await _repositoryManager.Team.GetTeamsByUserId(int.Parse(userId));
            List<TeamDto> response = _mapper.Map<List<TeamDto>>(usersTeams);
            return Ok(response);
        }

        [HttpGet("get-team-by-id/{teamId}"), Authorize]
        public async Task<ActionResult> GetTeamById([FromRoute] int teamId)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(teamId);
            if (team == null) return NotFound("Team not found.");

            TeamDto response = _mapper.Map<TeamDto>(team);
            return Ok(response);
        }

        [HttpGet("check-if-team-name-exists/{teamName}")]
        public async Task<ActionResult> CheckIfTeamNameExists([FromRoute] string teamName)
        {
            if (await _repositoryManager.Team.GetTeamByName(teamName) != null)
            {
                return Ok(new { exists = true });
            }
            return Ok(new { exists = false });
        }

        [HttpPost("create"), Authorize]
        public async Task<ActionResult> CreateTeam([FromBody] TeamUpsert request)
        {
            if (await _repositoryManager.Team.GetTeamByName(request.Name) != null)
            {
                return BadRequest($"Team name {request.Name} is already taken.");
            }
            if (request.Users.IsNullOrEmpty())
            {
                return BadRequest("A new team must have at least one user.");
            }
            List<User> teamUsers = await _repositoryManager.User.GetUsersByIds(request.Users.Select(user => user.Id), trackChanges: true);

            Team team = _mapper.Map<Team>(request);
            team.Users = teamUsers;
            await _repositoryManager.Team.CreateTeam(team);
            await _repositoryManager.SaveAsync();

            TeamDto response = _mapper.Map<TeamDto>(team);
            return Ok(response);
        }

        [HttpPost("add-users-to-team"), Authorize]
        public async Task<ActionResult> AddUsersToTeam([FromBody] TeamUpsert request)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(request.Id, true);
            if (team == null) return BadRequest("Team not found.");

            List<User> newUsers = await _repositoryManager.User.GetUsersByIds(request.Users.Select(user => user.Id), trackChanges: false);

            foreach (User user in newUsers)
            {
                team.Users.Add(user);
            }

            await _repositoryManager.SaveAsync();
            TeamDto response = _mapper.Map<TeamDto>(team);

            return Ok(response);
        }

        [HttpPost("remove-user-from-team"), Authorize]
        public async Task<ActionResult> RemoveUserFromTeam([FromBody] RemoveUserFromTeamRequest request)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(request.TeamId, trackChanges: true);
            if (team == null) return BadRequest("Team not found.");
            User? userToRemove = team.Users.SingleOrDefault(x => x.Id == request.UserId);
            if (userToRemove == null) return BadRequest("User not found in team users.");

            team.Users.Remove(userToRemove);
            await _repositoryManager.SaveAsync();

            TeamDto response = _mapper.Map<TeamDto>(team);
            return Ok(response);
        }

        [HttpGet("user-is-in-team/{teamId}"), Authorize]
        public async Task<ActionResult> UserIsInTeam([FromRoute] int teamId)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Team? team = await _repositoryManager.Team.GetTeamById(teamId);
            if (team is null) return BadRequest("Team not found.");

            // If the current user isn't in the given team, return unauthorized.
            if (!team.Users.Select(x => x.Id).ToList().Contains(int.Parse(userId)))
            {
                return Unauthorized();
            }

            TeamDto response = _mapper.Map<TeamDto>(team);
            return Ok(response);
        }

        [HttpDelete("delete-team-by-id/{teamId}")]
        public async Task<ActionResult> DeleteTeamById([FromRoute] int teamId)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(teamId);
            if (team is null) return BadRequest("Team not found.");

            _repositoryManager.Team.DeleteTeam(team);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }
    }
}
