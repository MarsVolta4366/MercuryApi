namespace MercuryApi.Data.Repository
{
    public interface ISprintRepository
    {
        Task CreateSprint(Sprint sprint);
    }

    public class SprintRepository : RepositoryBase<Sprint>, ISprintRepository
    {
        public SprintRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateSprint(Sprint sprint) =>
            await Create(sprint);
    }
}
