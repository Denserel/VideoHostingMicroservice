using System.ComponentModel.DataAnnotations;

namespace VideoServiceAPI.Models.Dtos;

public class VideoCreatDto
{
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
