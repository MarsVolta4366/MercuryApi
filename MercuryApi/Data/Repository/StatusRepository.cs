using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetAllStatuses(bool trackChanges = false);

        Task<Status?> GetStatusById(int id, bool trackChanges = false);
    }

    public class StatusRepository : RepositoryBase<Status>, IStatusRepository
    {
        public StatusRepository(MercuryDbContext context) : base(context) { }

        public async Task<IEnumerable<Status>> GetAllStatuses(bool trackChanges = false) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Status?> GetStatusById(int id, bool trackChanges = false) =>
            await FindByCondition(status => status.Id == id, trackChanges).FirstOrDefaultAsync();
    }
}
