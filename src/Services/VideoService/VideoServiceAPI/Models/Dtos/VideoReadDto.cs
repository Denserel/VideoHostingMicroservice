using System.ComponentModel.DataAnnotations;

namespace VideoServiceAPI.Models.Dtos;

public class VideoReadDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateLastEdit { get; set; } = DateTime.UtcNow;
}
