using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface ITeamRepository
    {
        Task<Team?> GetTeamByName(string name, bool trackChanges = false);
        Task CreateTeam(Team team);
    }

    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(MercuryDbContext context) : base(context) { }

        public async Task<Team?> GetTeamByName(string name, bool trackChanges = false) =>
            await FindByCondition(team => team.Name == name, trackChanges).FirstOrDefaultAsync();

        public async Task CreateTeam(Team team) =>
            await Create(team);
    }
}
