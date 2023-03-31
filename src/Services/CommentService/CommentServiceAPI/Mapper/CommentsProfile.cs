using AutoMapper;
using CommentServiceAPI.Models;
using CommentServiceAPI.Models.Dtos;

namespace CommentServiceAPI.Mapper;

public class CommentsProfile : Profile
{
    public CommentsProfile()
    {
        CreateMap<CommentModel, ReadCommentDto>();
        CreateMap<CreateCommentDto, CommentModel>();
        CreateMap<UpdateCommentDto, CommentModel>();
    }
}
