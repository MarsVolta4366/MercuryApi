using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        [HttpPost("create")]
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
