using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface ISprintRepository
    {
        Task CreateSprint(Sprint sprint);

        Task<Sprint?> GetSprintById(int sprintId, bool trackChanges = false);

        void DeleteSprint(Sprint sprint);
    }

    public class SprintRepository : RepositoryBase<Sprint>, ISprintRepository
    {
        public SprintRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateSprint(Sprint sprint) =>
            await Create(sprint);

        public async Task<Sprint?> GetSprintById(int sprintId, bool trackChanges = false) =>
            await FindByCondition(sprint => sprint.Id == sprintId, trackChanges)
                .Include(sprint => sprint.Tickets)
                .FirstOrDefaultAsync();

        public void DeleteSprint(Sprint sprint) =>
            Delete(sprint);
    }
}
