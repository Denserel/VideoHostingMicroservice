using Bogus;
using System.ComponentModel.DataAnnotations;

namespace VideoServiceAPI.Models.Dtos;

public class VideoUpdateDto
{
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    public static Faker<VideoUpdateDto> BogusVideoUpdateDto { get; } = new Faker<VideoUpdateDto>()
        .RuleFor(video => video.Url, fake => fake.Internet.Url())
        .RuleFor(video => video.Title, fake => fake.Random.Word())
        .RuleFor(video => video.Description, fake => fake.Random.Words());
}
