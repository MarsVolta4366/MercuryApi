using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface ITicketBusinessLogic
    {
        Task<TicketDto?> GetTicketById(int ticketId);
        Task<TicketDto> CreateTicket(TicketUpsert request);
        Task<TicketDto?> UpdateTicket(TicketUpsert request);
        Task DeleteTicketById(int ticketId);
    }

    public class TicketBusinessLogic : BusinessLogicBase, ITicketBusinessLogic
    {
        public TicketBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<TicketDto?> GetTicketById(int ticketId)
        {
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketById(ticketId);
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> CreateTicket(TicketUpsert request)
        {
            Ticket ticket = _mapper.Map<Ticket>(request);

            await _repositoryManager.Ticket.CreateTicket(ticket);
            await _repositoryManager.SaveAsync();

            if (request.UserId is int userId)
            {
                User? user = await _repositoryManager.User.GetUserById(userId);
                ticket.User = user;
            }

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto?> UpdateTicket(TicketUpsert request)
        {
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketById(request.Id, trackChanges: true);
            if (ticket == null) return null;

            // Map the update request over the entity to update fields.
            _mapper.Map(request, ticket);
            await _repositoryManager.SaveAsync();

            if (request.UserId is int userId)
            {
                User? user = await _repositoryManager.User.GetUserById(userId);
                ticket.User = user;
            }

            Status? status = await _repositoryManager.Status.GetStatusById(request.StatusId) ?? throw new Exception("Bad status.");
            ticket.Status = status;

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task DeleteTicketById(int ticketId)
        {
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketById(ticketId);
            if (ticket == null) return;

            _repositoryManager.Ticket.DeleteTicket(ticket);
            await _repositoryManager.SaveAsync();
        }
    }
}
