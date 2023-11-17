namespace MercuryApi.Data.Repository
{
    public interface ITicketRepository
    {
        Task CreateTicket(Ticket ticket);
    }

    public class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        public TicketRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateTicket(Ticket ticket) =>
            await Create(ticket);
    }
}
