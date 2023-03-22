using CommentServiceAPI.Models;

namespace CommentServiceAPI.Data.Repositories;
public interface ICommentRepository
{
    Task<CommentModel> GetCommentByIdAsync(string id);
    Task CreateCommentAsync(CommentModel comment);
    Task DeletCommentAsync(string id);
    Task<List<CommentModel>> GetCommentsOfContentAsync(string id);
    Task UpdateCommentAsync(CommentModel comment, string id);
}