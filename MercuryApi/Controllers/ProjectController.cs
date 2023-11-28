using MercuryApi.BLL;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.NonEntityDtos;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectBusinessLogic _projectBusinessLogic;

        public ProjectController(IProjectBusinessLogic projectBusinessLogic)
        {
            _projectBusinessLogic = projectBusinessLogic;
        }

        [HttpPost("check-if-project-name-exists")]
        public async Task<ActionResult> CheckIfProjectNameExists([FromBody] ProjectUpsert request)
        {
            ExistsDto response = await _projectBusinessLogic.ProjectNameExists(request);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateProject([FromBody] ProjectUpsert request)
        {
            ProjectDto? response = await _projectBusinessLogic.CreateProject(request);
            if (response == null) return BadRequest($"This team already has a project called {request.Name}.");

            return Ok(response);
        }

        [HttpGet("get-project-by-id/{projectId}")]
        public async Task<ActionResult> GetProjectById([FromRoute] int projectId)
        {
            ProjectDto? response = await _projectBusinessLogic.GetProjectById(projectId);

            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpDelete("delete-project-by-id/{projectId}")]
        public async Task<ActionResult> DeleteProjectById([FromRoute] int projectId)
        {
            await _projectBusinessLogic.DeleteProjectById(projectId);
            return NoContent();
        }
    }
}
