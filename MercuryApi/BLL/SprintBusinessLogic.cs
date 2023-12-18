using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface ISprintBusinessLogic
    {
        Task<SprintDto> CreateSprint(SprintUpsert request);
        Task DeleteSprint(int sprintId);
        Task<SprintDto?> UpdateSprint(SprintUpsert request);
    }

    public class SprintBusinessLogic : BusinessLogicBase, ISprintBusinessLogic
    {
        public SprintBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<SprintDto> CreateSprint(SprintUpsert request)
        {
            Sprint sprint = _mapper.Map<Sprint>(request);
            await _repositoryManager.Sprint.CreateSprint(sprint);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<SprintDto>(sprint);
        }

        public async Task DeleteSprint(int sprintId)
        {
            Sprint? sprint = await _repositoryManager.Sprint.GetSprintById(sprintId);
            if (sprint == null) return;

            foreach (Ticket ticket in sprint.Tickets)
            {
                ticket.SprintId = null;
            }
            _repositoryManager.Sprint.DeleteSprint(sprint);
            await _repositoryManager.SaveAsync();
        }

        public async Task<SprintDto?> UpdateSprint(SprintUpsert request)
        {
            Sprint? sprint = await _repositoryManager.Sprint.GetSprintById(request.Id, trackChanges: true);
            if (sprint == null) return null;

            // Map the update request over the entity to update fields.
            _mapper.Map(request, sprint);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<SprintDto>(sprint);
        }
    }
}
