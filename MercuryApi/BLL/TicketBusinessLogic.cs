﻿using AutoMapper;
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

            await IncludeChildren(ticket);

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto?> UpdateTicket(TicketUpsert request)
        {
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketById(request.Id, trackChanges: true);
            if (ticket == null) return null;

            // Map the update request over the entity to update fields.
            _mapper.Map(request, ticket);
            await _repositoryManager.SaveAsync();

            await IncludeChildren(ticket);

            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task DeleteTicketById(int ticketId)
        {
            Ticket? ticket = await _repositoryManager.Ticket.GetTicketById(ticketId);
            if (ticket == null) return;

            _repositoryManager.Ticket.DeleteTicket(ticket);
            await _repositoryManager.SaveAsync();
        }

        /// <summary>
        /// Attaches user, status, and sprint to a given ticket.
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task IncludeChildren(Ticket ticket)
        {
            // Attach user.
            if (ticket.UserId is int userId)
            {
                User? user = await _repositoryManager.User.GetUserById(userId);
                ticket.User = user;
            }

            // Attach status.
            Status status = await _repositoryManager.Status.GetStatusById(ticket.StatusId) ?? throw new Exception("Invalid status id.");
            ticket.Status = status;

            // Attaching sprint so that sprint name maps to ticket dto.
            if (ticket.SprintId is int sprintId)
            {
                Sprint? sprint = await _repositoryManager.Sprint.GetSprintById(sprintId);
                ticket.Sprint = sprint;
            }
        }
    }
}
