using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Entities;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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
            List<User> teamUsers = await _repositoryManager.User.GetUsersByIds(request.Users.Select(user => user.Id), trackChanges: true);

            Team team = _mapper.Map<Team>(request);
            team.Users = teamUsers;
            await _repositoryManager.Team.CreateTeam(team);
            await _repositoryManager.SaveAsync();
            return Ok("Team created");
        }
    }
}
