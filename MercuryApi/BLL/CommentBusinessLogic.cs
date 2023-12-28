using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;

namespace MercuryApi.BLL
{
    public interface ICommentBusinessLogic
    {
        Task<CommentDto> CreateComment(CommentUpsert request);
        Task DeleteComment(int commentId, int userId);
    }

    public class CommentBusinessLogic : BusinessLogicBase, ICommentBusinessLogic
    {
        public CommentBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<CommentDto> CreateComment(CommentUpsert request)
        {
            Comment comment = _mapper.Map<Comment>(request);
            comment.Date = DateTime.Now.Date;
            await _repositoryManager.Comment.CreateComment(comment);
            await _repositoryManager.SaveAsync();

            comment.User = await _repositoryManager.User.GetUserById(request.UserId) ?? throw new Exception("User not found.");

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task DeleteComment(int commentId, int userId)
        {
            Comment? comment = await _repositoryManager.Comment.GetCommentById(commentId);
            if (comment == null) return;
            if (comment.User.Id != userId) return;

            _repositoryManager.Comment.DeleteComment(comment);
            await _repositoryManager.SaveAsync();
        }
    }
}
