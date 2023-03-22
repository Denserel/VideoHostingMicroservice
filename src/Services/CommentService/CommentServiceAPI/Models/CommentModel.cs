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
}
