using MercuryApi.Data.Upserts;
using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface ITicketRepository
    {
        Task CreateTicket(Ticket ticket);
        Task<Ticket?> GetTicketWithChildrenById(int ticketId, bool trackChanges = false);
        Task<Ticket?> GetTicketById(int ticketId, bool trackChanges = false);
        void DeleteTicket(Ticket ticket);
        Task<List<Ticket>> GetTicketsInProjectSprint(int projectId, int? sprintId, bool trackChanges = false);
        Task<Ticket?> UpdateTicketOrder(TicketOrderUpsert request);
        Task<Ticket?> UpdateTicketOrderAndSprint(TicketOrderAndSprintUpsert request);
    }

    public class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        public TicketRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateTicket(Ticket ticket) =>
            await Create(ticket);

        public async Task<Ticket?> GetTicketWithChildrenById(int ticketId, bool trackChanges = false) =>
            await FindByCondition(ticket => ticket.Id == ticketId, trackChanges)
                .Include(ticket => ticket.Sprint)
                .Include(ticket => ticket.Status)
                .Include(ticket => ticket.User)
                .Include(ticket => ticket.Comments)
                    .ThenInclude(comment => comment.User)
                .FirstOrDefaultAsync();

        public async Task<Ticket?> GetTicketById(int ticketId, bool trackChanges = false) =>
            await FindByCondition(ticket => ticket.Id == ticketId, trackChanges)
                .FirstOrDefaultAsync();

        public void DeleteTicket(Ticket ticket) =>
            Delete(ticket);

        public async Task<List<Ticket>> GetTicketsInProjectSprint(int projectId, int? sprintId, bool trackChanges = false) =>
            await FindByCondition(ticket => ticket.SprintId == sprintId && ticket.ProjectId == projectId, trackChanges).ToListAsync();

        public async Task<Ticket?> UpdateTicketOrder(TicketOrderUpsert request) =>
            (await RunSqlRaw($"UpdateTicketOrder @TicketId={request.TicketId}, @NewOrderValue={request.NewOrderValue}").ToListAsync()).FirstOrDefault();

        public async Task<Ticket?> UpdateTicketOrderAndSprint(TicketOrderAndSprintUpsert request) =>
            (await RunSqlRaw($"UpdateTicketOrderAndSprint @TicketId={request.TicketId}, @NewOrderValue={request.NewOrderValue}, @NewSprintId={(request.NewSprintId != null ? request.NewSprintId : "NULL")}").ToListAsync()).FirstOrDefault();
    }
}
