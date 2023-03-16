using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VideoServiceAPI.Data;
using VideoServiceAPI.Data.Repositories;
using VideoServiceAPI.Models;
using VideoServiceAPI.Models.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

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

static async Task<IResult> GetAllVideos(IVideoRepository repository, ILogger<Program> logger, IMapper mapper)
{
    logger.LogInformation("Geting all the videos");
    return TypedResults.Ok(mapper.Map<IEnumerable<VideoReadDto>>(await repository.GetAllVideosAsync()));
}

static async Task<IResult> GetVideo(IVideoRepository repository, ILogger<Program> logger, IMapper mapper, int id)
{
    if (await repository.GetVideoByIdAsync(id) is VideoModel video)
    {
        logger.LogInformation("I found a video with the id: {Id}", id);
        return TypedResults.Ok(mapper.Map<VideoReadDto>(video));
    }

    logger.LogError("Nothing here with the id: {Id}", id);
    return TypedResults.NotFound();
}

static async Task<IResult> CreateVideo(IVideoRepository repository, ILogger<Program> logger, IMapper mapper, VideoCreatDto videoCreat)
{
    var videoModel = mapper.Map<VideoModel>(videoCreat);

    await repository.CreateVideoAsync(videoModel);
    await repository.SaveChangesAsync();

    var videoReadDto = mapper.Map<VideoReadDto>(videoModel);

    logger.LogInformation("Created video with id: {Id}", videoReadDto.Id);
    return TypedResults.Created($"/videos/{videoReadDto.Id}", videoReadDto);
}

static async Task<IResult> UpdateVideo(IVideoRepository repository, ILogger<Program> logger, IMapper mapper, VideoUpdateDto videoUpdate, int id)
{
    var video = await repository.GetVideoByIdAsync(id);

    if (video is null)
    {
        logger.LogError("Nothing here with the id: {Id}", id);
        return TypedResults.NotFound();
    }

    mapper.Map(videoUpdate, video);

    await repository.SaveChangesAsync();

    logger.LogInformation("Updated video with id: {Id}", id);
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteVideo(IVideoRepository repository, ILogger<Program> logger, int id)
{
    if (await repository.GetVideoByIdAsync(id) is VideoModel video)
    {
        repository.DeleteVideo(video);
        await repository.SaveChangesAsync();

        logger.LogInformation("Deleted video with id: {id}", id);
        return TypedResults.Ok(video);
    }

    logger.LogError("Nothing here with the id: {Id}", id);
    return TypedResults.NotFound();
}
