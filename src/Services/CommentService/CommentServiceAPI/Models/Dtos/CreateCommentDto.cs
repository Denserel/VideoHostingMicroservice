namespace CommentServiceAPI.Models.Dtos;

public class CreateCommentDto
{
    public string ContentId { get; set; }
    public string CommentText { get; set; } = string.Empty;
}
