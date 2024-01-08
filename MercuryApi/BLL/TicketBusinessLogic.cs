using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface ITicketBusinessLogic
    {
        Task<TicketDto?> GetTicketById(int ticketId);
        Task<TicketDto> CreateTicket(TicketCreate request);
        Task<TicketDto?> UpdateTicket(TicketUpdate request);
        Task DeleteTicketById(int ticketId);
        Task<TicketDto?> UpdateTicketOrder(TicketOrderUpsert request);
        Task<TicketDto?> UpdateTicketOrderAndSprint(TicketOrderAndSprintUpsert request);
    }

    public class TicketBusinessLogic : BusinessLogicBase, ITicketBusinessLogic
    {
        public TicketBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<TicketDto?> GetTicketById(int ticketId)
        {
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketWithChildrenById(ticketId);
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> CreateTicket(TicketCreate request)
        {
            Ticket ticket = _mapper.Map<Ticket>(request);

            ticket.Order = await GetTicketOrder(ticket.ProjectId, request.SprintId);

            await _repositoryManager.Ticket.CreateTicket(ticket);
            await _repositoryManager.SaveAsync();

            Ticket? createdTicket = await _repositoryManager.Ticket.GetTicketWithChildrenById(ticket.Id);
            return _mapper.Map<TicketDto>(createdTicket);
        }

        public async Task<TicketDto?> UpdateTicket(TicketUpdate request)
        {
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketWithChildrenById(request.Id, trackChanges: true);
            if (ticket == null) return null;

            // If ticket is changing sprints, set ticket order to last.
            if (request.SprintId != ticket.SprintId)
            {
                ticket.Order = await GetTicketOrder(ticket.ProjectId, request.SprintId);
            }

            // Map the update request over the entity to update fields.
            _mapper.Map(request, ticket);
            await _repositoryManager.SaveAsync();

            Ticket? updatedTicket = await _repositoryManager.Ticket.GetTicketWithChildrenById(request.Id);

            return _mapper.Map<TicketDto>(updatedTicket);
        }

        public async Task DeleteTicketById(int ticketId)
        {
            // GetTicketById doesn't include children, need it this way for delete because if multiple comments are included
            // by the same user, an exception gets thrown that you can't track two instances of the same entity on delete.
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketById(ticketId);
            if (ticket == null) return;

            _repositoryManager.Ticket.DeleteTicket(ticket);
            await _repositoryManager.SaveAsync();
        }

        public async Task<TicketDto?> UpdateTicketOrder(TicketOrderUpsert request)
        {
            Ticket? ticket = await _repositoryManager.Ticket.UpdateTicketOrder(request);

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto?> UpdateTicketOrderAndSprint(TicketOrderAndSprintUpsert request)
        {
            await _repositoryManager.Ticket.UpdateTicketOrderAndSprint(request);

            Ticket? ticket = await _repositoryManager.Ticket.GetTicketWithChildrenById(request.TicketId);

            return _mapper.Map<TicketDto>(ticket);
        }

        private async Task<int> GetTicketOrder(int projectId, int? sprintId)
        {
            List<Ticket> sprintTickets = await _repositoryManager.Ticket.GetTicketsInProjectSprint(projectId, sprintId);
            sprintTickets = sprintTickets.OrderBy(x => x.Order).ToList();
            return sprintTickets.Count > 0 ? sprintTickets.Last().Order + 1 : 0;
        }
    }
}
