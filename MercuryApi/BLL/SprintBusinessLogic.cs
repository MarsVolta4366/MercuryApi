using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface ISprintBusinessLogic
    {
        Task<SprintDto> CreateSprint(SprintUpsert request);
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
    }
}
