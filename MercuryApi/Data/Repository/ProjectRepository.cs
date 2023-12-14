using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsByTeamId(int teamId, bool trackChanges = false);
        Task CreateProject(Project project);
        Task<Project?> GetProjectById(int projectId, bool trackChanges = false);
        void DeleteProject(Project project);
    }

    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(MercuryDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> GetProjectsByTeamId(int teamId, bool trackChanges = false) =>
            await FindByCondition(project => project.TeamId.Equals(teamId), trackChanges).ToListAsync();

        public async Task CreateProject(Project project) =>
            await Create(project);

        public async Task<Project?> GetProjectById(int projectId, bool trackChanges = false) =>
            await FindByCondition(project => project.Id == projectId, trackChanges)
                .Include(project => project.Sprints)
                //    .ThenInclude(sprint => sprint.Tickets)
                //        .ThenInclude(ticket => ticket.User)
                //.Include(project => project.Sprints)
                //    .ThenInclude(sprint => sprint.Tickets)
                //        .ThenInclude(ticket => ticket.Status)

                // Only include tickets that aren't in a sprint (backlog tickets).
                //.Include(project => project.Tickets.Where(ticket => ticket.SprintId == null))
                .Include(project => project.Tickets)
                    .ThenInclude(ticket => ticket.User)
                .Include(project => project.Tickets)
                    .ThenInclude(ticket => ticket.Status)
                .Include(project => project.Tickets)
                    .ThenInclude(ticket => ticket.Sprint)
                .FirstOrDefaultAsync();

        public void DeleteProject(Project project) =>
            Delete(project);
    }
}
