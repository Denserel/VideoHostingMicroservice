using Bogus;
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
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    public static Faker<VideoModel> BogusVideoModel { get; } = new Faker<VideoModel>()
        .RuleFor(video => video.Id, fake => fake.Random.Number())
        .RuleFor(video => video.Url, fake => fake.Internet.Url())
        .RuleFor(video => video.Title, fake => fake.Random.Word())
        .RuleFor(video => video.Description, fake => fake.Random.Words());
}