using Bogus;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommentServiceAPI.Models;

public class CommentModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ContentId { get; set; }
    public string CommentText { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    public static Faker<CommentModel> BogusCommentModel { get; } = new Faker<CommentModel>()
        .RuleFor(comment => comment.Id, fake => fake.Random.Number().ToString())
        .RuleFor(comment => comment.ContentId, fake => fake.Random.Number().ToString())
        .RuleFor(comment => comment.CommentText, fake => fake.Random.Words());
}
