namespace MercuryApi.Data.Repository
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        ITeamRepository Team { get; }
        IProjectRepository Project { get; }
        Task SaveAsync();
    }

    public class RepositoryManager : IRepositoryManager
    {
        private readonly MercuryDbContext _context;
        private IUserRepository? _userRepository;
        private ITeamRepository? _teamRepository;
        private IProjectRepository? _projectRepository;

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

        public ITeamRepository Team
        {
            get
            {
                _teamRepository ??= new TeamRepository(_context);
                return _teamRepository;
            }
        }

        public IProjectRepository Project
        {
            get
            {
                _projectRepository ??= new ProjectRepository(_context);
                return _projectRepository;
            }
        }

        public Task SaveAsync() => _context.SaveChangesAsync();
    }
}
