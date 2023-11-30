using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetAllStatuses(bool trackChanges = false);
    }

    public class StatusRepository : RepositoryBase<Status>, IStatusRepository
    {
        public StatusRepository(MercuryDbContext context) : base(context) { }

        public async Task<IEnumerable<Status>> GetAllStatuses(bool trackChanges = false) =>
            await FindAll(trackChanges).ToListAsync();
    }
}
