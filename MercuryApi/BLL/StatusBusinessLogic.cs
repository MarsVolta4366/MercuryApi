using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;

namespace MercuryApi.BLL
{
    public interface IStatusBusinessLogic
    {
        Task<IEnumerable<StatusDto>> GetAllStatuses();
    }

    public class StatusBusinessLogic : BusinessLogicBase, IStatusBusinessLogic
    {
        public StatusBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<IEnumerable<StatusDto>> GetAllStatuses()
        {
            IEnumerable<Status> statuses = await _repositoryManager.Status.GetAllStatuses();
            return _mapper.Map<IEnumerable<StatusDto>>(statuses);
        }
    }
}
