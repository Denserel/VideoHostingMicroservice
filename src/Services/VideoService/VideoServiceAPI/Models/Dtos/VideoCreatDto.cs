using Bogus;
using System.ComponentModel.DataAnnotations;

namespace VideoServiceAPI.Models.Dtos;

public class VideoCreatDto
{
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public static Faker<VideoCreatDto> BobusVideoCreateDto { get; } = new Faker<VideoCreatDto>()
        .RuleFor(video => video.Url, fake => fake.Internet.Url())
        .RuleFor(video => video.Title, fake => fake.Random.Word())
        .RuleFor(video => video.Description, fake => fake.Random.Words());
}
