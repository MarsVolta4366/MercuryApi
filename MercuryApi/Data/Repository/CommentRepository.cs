namespace MercuryApi.Data.Repository
{
    public interface ICommentRepository
    {
        Task CreateComment(Comment comment);
    }

    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(MercuryDbContext context) : base(context) { }

        public async Task CreateComment(Comment comment) =>
            await Create(comment);
    }
}
