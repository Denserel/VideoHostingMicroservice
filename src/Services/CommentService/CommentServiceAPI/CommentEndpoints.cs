using AutoMapper;
using CommentServiceAPI.Data.Repositories;
using CommentServiceAPI.Models;
using CommentServiceAPI.Models.Dtos;
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

    public static async Task<IResult> GetCommentsOfContent(ICommentRepository repository, IMapper mapper, string contentId)
    {
        return TypedResults.Ok(mapper.Map<List<ReadCommentDto>>(await repository.GetCommentsOfContentAsync(contentId)));
    }

    public static async Task<IResult> CreateComment(ICommentRepository repository, IMapper mapper, CreateCommentDto creatComment)
    {
        var comment = mapper.Map<CommentModel>(creatComment);

        await repository.CreateCommentAsync(comment);
        return TypedResults.Created(" ", comment);
    }

    public static async Task<IResult> UpdateComment(ICommentRepository repository, IMapper mapper, UpdateCommentDto updateComment, string id)
    {
        if (await repository.GetCommentByIdAsync(id) is CommentModel comment)
        {
            await repository.UpdateCommentAsync(mapper.Map(updateComment, comment), id);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    public static async Task<IResult> DeleteComment(ICommentRepository repository, string id)
    {
        if (await repository.GetCommentByIdAsync(id) is CommentModel comment)
        {
            await repository.DeletCommentAsync(id);
            return TypedResults.Ok(comment);
        }

        return TypedResults.NotFound();
    }
}
