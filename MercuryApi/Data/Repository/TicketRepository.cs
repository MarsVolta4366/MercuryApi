using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface ITicketRepository
    {
        Task CreateTicket(Ticket ticket);
        Task<Ticket?> GetTicketById(int ticketId, bool trackChanges = false);
    }

    public class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        public TicketRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateTicket(Ticket ticket) =>
            await Create(ticket);

        public async Task<Ticket?> GetTicketById(int ticketId, bool trackChanges = false) =>
            await FindByCondition(ticket => ticket.Id == ticketId, trackChanges)
                .Include(ticket => ticket.User)
                .FirstOrDefaultAsync();
    }
}
