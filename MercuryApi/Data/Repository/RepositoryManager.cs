namespace MercuryApi.Data.Repository
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        Task SaveAsync();
    }

    public class RepositoryManager : IRepositoryManager
    {
        private readonly MercuryDbContext _context;
        private IUserRepository? _userRepository;

        public RepositoryManager(MercuryDbContext context)
        {
            _context = context;
        }

        public IUserRepository User
        {
            get
            {
                _userRepository ??= new UserRepository(_context);
                return _userRepository;
            }
        }

        public Task SaveAsync() => _context.SaveChangesAsync();
    }
}
