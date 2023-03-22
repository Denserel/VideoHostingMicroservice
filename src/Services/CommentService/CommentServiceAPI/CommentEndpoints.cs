using CommentServiceAPI.Data.Repositories;
using CommentServiceAPI.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Runtime.CompilerServices;

namespace CommentServiceAPI;

public static class CommentEndpoints
{
    public static RouteGroupBuilder MapCommantApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{contentId}", GetCommentsOfContent);
        group.MapPost("/", CreateComment);
        group.MapPut("/{Id}", UpdateComment);
        group.MapDelete("/{id}", DeleteComment);

        return group;
    }

    public static async Task<IResult> GetCommentsOfContent(ICommentRepository repository, string contentId)
    {
        return TypedResults.Ok(await repository.GetCommentsOfContentAsync(contentId));
    }

    public static async Task<IResult> CreateComment(ICommentRepository repository, CommentModel comment)
    {
        await repository.CreateCommentAsync(comment);
        return TypedResults.Created(string.Empty, comment);
    }

    public static async Task<IResult> UpdateComment(ICommentRepository repository, CommentModel updateComment, string id)
    {
        var comment = await repository.GetCommentByIdAsync(id);

        if (comment == null)
        {
            return TypedResults.NotFound();
        }

        updateComment.Id = comment.Id;

        await repository.UpdateCommentAsync(updateComment, id);

        return TypedResults.NoContent();
    }

    public static async Task<IResult> DeleteComment(ICommentRepository repository, string id)
    {
        var comment = await repository.GetCommentByIdAsync(id);

        if (comment == null)
        {
            return TypedResults.NotFound();
        }

        await repository.DeletCommentAsync(id);
        
        return TypedResults.NoContent();
    }
}
