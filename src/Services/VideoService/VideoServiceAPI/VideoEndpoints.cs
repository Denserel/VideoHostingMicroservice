using AutoMapper;
using VideoServiceAPI.Data.Repositories;
using VideoServiceAPI.Models.Dtos;
using VideoServiceAPI.Models;

namespace VideoServiceAPI;

public static class VideoEndpoints
{
    public static RouteGroupBuilder MapVideosApi (this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllVideos);
        group.MapGet("/{id}", GetVideo);
        group.MapPost("/", CreateVideo);
        group.MapPut("/{id}", UpdateVideo);
        group.MapDelete("/{id}", DeleteVideo);

        return group;
    }

    public static async Task<IResult> GetAllVideos(IVideoRepository repository, IMapper mapper)
    {
        return TypedResults.Ok(mapper.Map<IEnumerable<VideoReadDto>>(await repository.GetAllVideosAsync()));
    }

    public static async Task<IResult> GetVideo(IVideoRepository repository, IMapper mapper, int id)
    {
        return await repository.GetVideoByIdAsync(id) is VideoModel video ? TypedResults.Ok(mapper.Map<VideoReadDto>(video)) : TypedResults.NotFound();
    }

    public static async Task<IResult> CreateVideo(IVideoRepository repository, IMapper mapper, VideoCreatDto videoCreat)
    {
        var videoModel = mapper.Map<VideoModel>(videoCreat);

        // TODO Upload to AzureBlobStorage

        await repository.CreateVideoAsync(videoModel);
        await repository.SaveChangesAsync();

        var videoReadDto = mapper.Map<VideoReadDto>(videoModel);

        return TypedResults.Created($"/video/{videoReadDto.Id}", videoReadDto);
    }

    public static async Task<IResult> UpdateVideo(IVideoRepository repository, IMapper mapper, VideoUpdateDto videoUpdate, int id)
    {
        var video = await repository.GetVideoByIdAsync(id);

        if (video is null)
        {
            return TypedResults.NotFound();
        }

        mapper.Map(videoUpdate, video);

        await repository.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    public static async Task<IResult> DeleteVideo(IVideoRepository repository, int id)
    {
        if (await repository.GetVideoByIdAsync(id) is VideoModel video)
        {
            repository.DeleteVideo(video);
            await repository.SaveChangesAsync();
            return TypedResults.Ok(video);
        }

        return TypedResults.NotFound();
    }
}
