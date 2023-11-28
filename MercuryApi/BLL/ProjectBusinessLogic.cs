using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.NonEntityDtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface IProjectBusinessLogic
    {
        Task<ExistsDto> ProjectNameExists(ProjectUpsert request);
        Task<ProjectDto?> CreateProject(ProjectUpsert request);
        Task<ProjectDto?> GetProjectById(int projectId);
        Task DeleteProjectById(int projectId);
    }

    public class ProjectBusinessLogic : BusinessLogicBase, IProjectBusinessLogic
    {
        public ProjectBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<ExistsDto> ProjectNameExists(ProjectUpsert request)
        {
            IEnumerable<Project> projects = await _repositoryManager.Project.GetProjectsByTeamId(request.TeamId);

            if (projects.Select(p => p.Name).Contains(request.Name))
            {
                return new() { Exists = true };
            }
            return new() { Exists = false };
        }

        public async Task<ProjectDto?> CreateProject(ProjectUpsert request)
        {
            // If the request team already has a project with the request project name, return null.
            if ((await ProjectNameExists(request)).Exists)
            {
                return null;
            }

            Project project = _mapper.Map<Project>(request);
            await _repositoryManager.Project.CreateProject(project);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> GetProjectById(int projectId)
        {
            Project? project = await _repositoryManager.Project.GetProjectById(projectId);
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task DeleteProjectById(int projectId)
        {
            Project? project = await _repositoryManager.Project.GetProjectById(projectId);
            if (project == null) return;

            _repositoryManager.Project.DeleteProject(project);
            await _repositoryManager.SaveAsync();
        }
    }
}
