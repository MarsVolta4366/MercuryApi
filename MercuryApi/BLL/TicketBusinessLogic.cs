using AutoMapper;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface ITicketBusinessLogic
    {
        Task CreateTicket(TicketUpsert request);
    }

    public class TicketBusinessLogic : BusinessLogicBase, ITicketBusinessLogic
    {
        public TicketBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task CreateTicket(TicketUpsert request)
        {
            Ticket ticket = _mapper.Map<Ticket>(request);
            await _repositoryManager.Ticket.CreateTicket(ticket);
            await _repositoryManager.SaveAsync();
        }
    }
}
