using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ProjectController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpPost("check-if-project-name-exists")]
        public async Task<ActionResult> CheckIfProjectNameExists([FromBody] ProjectUpsert request)
        {
            IEnumerable<Project> teamProjects = await _repositoryManager.Project.GetProjectsByTeamId(request.TeamId, trackChanges: false);

            // If the request.team already has a project with the request.name, return bad request.
            if (teamProjects.Select(p => p.Name).Contains(request.Name))
            {
                return Ok(new { exists = true });
            }
            return Ok(new { exists = false });
        }

        [HttpPost("create"), Authorize]
        public async Task<ActionResult> CreateProject([FromBody] ProjectUpsert request)
        {
            IEnumerable<Project> teamProjects = await _repositoryManager.Project.GetProjectsByTeamId(request.TeamId, trackChanges: false);

            // If the request.team already has a project with the request.name, return bad request.
            if (teamProjects.Select(p => p.Name).Contains(request.Name))
            {
                return BadRequest($"This team already has a project called {request.Name}.");
            }

            Project project = _mapper.Map<Project>(request);
            await _repositoryManager.Project.CreateProject(project);
            await _repositoryManager.SaveAsync();

            ProjectDto response = _mapper.Map<ProjectDto>(project);

            return Ok(response);
        }
    }
}
