namespace CommentServiceAPI.Models.Dtos;

public class ReadCommentDto
{
    public string CommentText { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
}
