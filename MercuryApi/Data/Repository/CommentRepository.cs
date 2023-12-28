using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data.Repository
{
    public interface ICommentRepository
    {
        Task CreateComment(Comment comment);
        Task<Comment?> GetCommentById(int commentId, bool trackChanges = false);
        void DeleteComment(Comment comment);
    }

    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateComment(Comment comment) =>
            await Create(comment);

        public async Task<Comment?> GetCommentById(int commentId, bool trackChanges = false) =>
            await FindByCondition(comment => comment.Id == commentId, trackChanges)
                .Include(comment => comment.User)
                .FirstOrDefaultAsync();

        public void DeleteComment(Comment comment) =>
            Delete(comment);
    }
}
