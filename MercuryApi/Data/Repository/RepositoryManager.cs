namespace MercuryApi.Data.Repository
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        ITeamRepository Team { get; }
        IProjectRepository Project { get; }
        ITicketRepository Ticket { get; }
        IStatusRepository Status { get; }
        ISprintRepository Sprint { get; }
        Task SaveAsync();
    }

    public class RepositoryManager : IRepositoryManager
    {
        private readonly MercuryDbContext _context;
        private IUserRepository? _userRepository;
        private ITeamRepository? _teamRepository;
        private IProjectRepository? _projectRepository;
        private ITicketRepository? _ticketRepository;
        private IStatusRepository? _statusRepository;
        private ISprintRepository? _sprintRepository;

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

        public ITicketRepository Ticket
        {
            get
            {
                _ticketRepository ??= new TicketRepository(_context);
                return _ticketRepository;
            }
        }

        public IStatusRepository Status
        {
            get
            {
                _statusRepository ??= new StatusRepository(_context);
                return _statusRepository;
            }
        }

        public ISprintRepository Sprint
        {
            get
            {
                _sprintRepository ??= new SprintRepository(_context);
                return _sprintRepository;
            }
        }

        public Task SaveAsync() => _context.SaveChangesAsync();
    }
}
