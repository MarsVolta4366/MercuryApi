using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(bool trackChanges);
        Task<User?> GetUserByUsername(string username, bool trackChanges);
        Task<User?> GetUserById(int userId, bool trackChanges);
        Task<List<User>> GetUsersByIds(IEnumerable<int> userIds, bool trackChanges);
        Task CreateUser(User user);
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(MercuryDbContext context) : base(context) { }

        public async Task<IEnumerable<User>> GetAllUsers(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<User?> GetUserByUsername(string username, bool trackChanges) =>
            await FindByCondition(user => user.Username == username, trackChanges).FirstOrDefaultAsync();

        public async Task<User?> GetUserById(int userId, bool trackChanges) =>
            await FindByCondition(user => user.Id == userId, trackChanges).FirstOrDefaultAsync();

        public async Task<List<User>> GetUsersByIds(IEnumerable<int> userIds, bool trackChanges) =>
            await FindByCondition(user => userIds.Contains(user.Id), trackChanges).ToListAsync();

        public async Task CreateUser(User user) =>
            await Create(user);
    }
}
