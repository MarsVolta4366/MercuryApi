namespace MercuryApi.Data.Repository
{
    public interface ITeamRepository
    {
        Task CreateTeam(Team team);
    }

    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateTeam(Team team) =>
            await Create(team);
    }
}
