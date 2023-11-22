using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface ITeamRepository
    {
        Task<Team?> GetTeamById(int teamId, bool trackChanges = false);
        Task<Team?> GetTeamByName(string name, bool trackChanges = false);
        Task<List<Team>> GetTeamsByUserId(int userId, bool trackChanges = false);
        Task CreateTeam(Team team);
        void DeleteTeam(Team teamId);
    }

    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(MercuryDbContext context) : base(context) { }

        public async Task<Team?> GetTeamById(int teamId, bool trackChanges = false) =>
            await FindByCondition(team => team.Id == teamId, trackChanges)
                .Include(team => team.Projects)
                    .ThenInclude(project => project.Tickets)
                .Include(team => team.Users)
                .FirstOrDefaultAsync();

        public async Task<Team?> GetTeamByName(string name, bool trackChanges = false) =>
            await FindByCondition(team => team.Name == name, trackChanges).FirstOrDefaultAsync();

        public async Task<List<Team>> GetTeamsByUserId(int userId, bool trackChanges = false) =>
            await FindByCondition(team => team.Users.Select(user => user.Id).Contains(userId), trackChanges)
                .Include(team => team.Users).ToListAsync();

        public async Task CreateTeam(Team team) =>
            await Create(team);

        public void DeleteTeam(Team team) =>
            Delete(team);
    }
}
