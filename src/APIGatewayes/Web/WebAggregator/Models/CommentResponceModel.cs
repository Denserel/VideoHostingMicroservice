namespace WebAggregator.Models;

public class CommentResponceModel
{
    public string Id { get; set; }
    public string ContentId { get; set; }
    public string CommentText { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
}
