using CommentServiceAPI.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentServiceAPI.Data.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly IMongoCollection<CommentModel> collection;

    public CommentRepository(IOptions<CommentDatabaseSettings> commentDatabaseSettings)
    {
        var mongoClient = new MongoClient(commentDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(commentDatabaseSettings.Value.DatabaseName);
        collection = mongoDatabase.GetCollection<CommentModel>(commentDatabaseSettings.Value.CollectionName);
    }

    public async Task<CommentModel> GetCommentByIdAsync(string id)
    {
        return await collection.Find(comment => comment.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<CommentModel>> GetCommentsOfContentAsync(string id)
    {
        return await collection.Find(comment => comment.ContentId == id).ToListAsync();
    }

    public async Task CreateCommentAsync(CommentModel comment)
    {
        await collection.InsertOneAsync(comment);
    }

    public async Task UpdateCommentAsync(CommentModel comment, string id)
    {
        await collection.ReplaceOneAsync(comment => comment.Id == id, comment);
    }

    public async Task DeletCommentAsync(string id)
    {
        await collection.DeleteOneAsync(comment => comment.Id == id);
    }
}
