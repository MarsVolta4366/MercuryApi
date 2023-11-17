using AutoMapper;
using MercuryApi.Data.Repository;

namespace MercuryApi.BLL
{
    public abstract class BusinessLogicBase
    {
        protected readonly IRepositoryManager _repositoryManager;
        protected readonly IMapper _mapper;

        public BusinessLogicBase(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
    }
}
