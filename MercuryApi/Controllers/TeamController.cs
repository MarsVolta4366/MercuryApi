using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
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
            if (team == null) return BadRequest("Team not found.");

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
    }
}
