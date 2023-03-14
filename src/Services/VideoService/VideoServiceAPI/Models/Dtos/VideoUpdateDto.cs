using System.ComponentModel.DataAnnotations;

namespace VideoServiceAPI.Models.Dtos;

public class VideoUpdateDto
{
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateLastEdit { get; set; } = DateTime.UtcNow;
}
