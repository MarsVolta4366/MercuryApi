using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface ITicketBusinessLogic
    {
        Task<TicketDto> CreateTicket(TicketUpsert request);
    }

    public class TicketBusinessLogic : BusinessLogicBase, ITicketBusinessLogic
    {
        public TicketBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<TicketDto> CreateTicket(TicketUpsert request)
        {
            Ticket ticket = _mapper.Map<Ticket>(request);
            await _repositoryManager.Ticket.CreateTicket(ticket);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<TicketDto>(ticket);
        }
    }
}
