using AutoMapper;
using VideoServiceAPI.Models;
using VideoServiceAPI.Models.Dtos;

namespace VideoServiceAPI.Mapper.Profiles;

public class VideosProfile : Profile
{
    public VideosProfile()
    {
        CreateMap<VideoModel, VideoReadDto>();
        CreateMap<VideoCreatDto, VideoModel>();
        CreateMap<VideoUpdateDto, VideoModel>();
    }
}
