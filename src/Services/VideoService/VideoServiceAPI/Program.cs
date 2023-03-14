using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using VideoServiceAPI.Data;
using VideoServiceAPI.Data.Repositories;
using VideoServiceAPI.Models;
using VideoServiceAPI.Models.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
    options=> options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

var videos = app.MapGroup("/videos");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

videos.MapGet("/", GetAllVideos);
videos.MapGet("/{id}", GetVideo);
videos.MapPost("/", CreateVideo);
videos.MapPut("/{id}", UpdateVideo);
videos.MapDelete("/{id}", DeleteVideo);

app.Run();

static async Task<IResult> GetAllVideos (IVideoRepository repository, IMapper mapper)
{
    return TypedResults.Ok(mapper.Map<IEnumerable<VideoReadDto>>(await repository.GetAllVideosAsync()));
}

static async Task<IResult> GetVideo (IVideoRepository repository, IMapper mapper, int id)
{
    return await repository.GetVideoByIdAsync(id) is VideoModel video ? 
        TypedResults.Ok(mapper.Map<VideoReadDto>(video)) : TypedResults.NotFound();
}

static async Task<IResult> CreateVideo (IVideoRepository repository, IMapper mapper, VideoCreatDto videoCreat)
{
    var videoModel = mapper.Map<VideoModel>(videoCreat);

    await repository.CreateVideoAsync(videoModel);
    await repository.SaveChangesAsync();

    var videoReadDto = mapper.Map<VideoReadDto>(videoModel);

    return TypedResults.Created($"/videos/{videoReadDto.Id}", videoReadDto);
}

static async Task<IResult> UpdateVideo (IVideoRepository repository, IMapper mapper, VideoUpdateDto videoUpdate, int id)
{
    var video = await repository.GetVideoByIdAsync(id);
    
    if (video is null) return TypedResults.NotFound();

    mapper.Map(videoUpdate, video);

    await repository.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteVideo (IVideoRepository repository, int id)
{
    if (await repository.GetVideoByIdAsync(id) is VideoModel video)
    {
        repository.DeleteVideo(video);
        await repository.SaveChangesAsync();
        return TypedResults.Ok(video);
    }

    return TypedResults.NotFound();
}
