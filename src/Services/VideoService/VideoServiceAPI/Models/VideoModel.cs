using System.ComponentModel.DataAnnotations;

namespace VideoServiceAPI.Models;

public class VideoModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateLastEdit { get; set; } = DateTime.UtcNow;
}
