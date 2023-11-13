using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsByTeamId(int teamId, bool trackChanges);
        Task CreateProject(Project project);
    }

    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(MercuryDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> GetProjectsByTeamId(int teamId, bool trackChanges) =>
            await FindByCondition(project => project.TeamId.Equals(teamId), trackChanges).ToListAsync();

        public async Task CreateProject(Project project) =>
            await Create(project);
    }
}
