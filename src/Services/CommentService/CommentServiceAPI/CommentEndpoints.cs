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

    public static async Task<IResult> DeleteComment(int id)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> UpdateComment(int id)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> CreateComment()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> GetCommentsOfContent(int contentId)
    {
        throw new NotImplementedException();
    }
}
