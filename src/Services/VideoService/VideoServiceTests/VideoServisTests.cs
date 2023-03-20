using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoServiceAPI;
using VideoServiceAPI.Data.Repositories;
using VideoServiceAPI.Mapper;
using VideoServiceAPI.Models;
using VideoServiceAPI.Models.Dtos;

namespace VideoServiceTests;
public class VideoServisTests
{
    private readonly Mock<IVideoRepository> mockVideoRepository = new Mock<IVideoRepository>();
    private readonly IMapper mapper;

    public VideoServisTests ()
    {
        var mockMapper = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile(new VideosProfile());
        });

        mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task GetAllVideos_ReturnsOkWithAllVideos()
    {
        var bogusVideos = VideoModel.BogusVideoModel.Generate(3).AsEnumerable();

        mockVideoRepository.Setup(repository => repository.GetAllVideosAsync())
            .ReturnsAsync(bogusVideos);

        var result = (Ok<IEnumerable<VideoReadDto>>) await VideoEndpoints.GetAllVideos(mockVideoRepository.Object, mapper);
        
        Assert.Equal(200, result.StatusCode);

        var list = Assert.IsAssignableFrom<IEnumerable<VideoReadDto>>(result.Value);

        Assert.Equal(3, list.ToList().Count);

    }

    [Fact]
    public async Task GetVideo_WithId_ReturnsOkWithVideo()
    {
        var bogusVideo = VideoModel.BogusVideoModel.Generate();

        mockVideoRepository.Setup(repository => repository.GetVideoByIdAsync(bogusVideo.Id))
            .ReturnsAsync(bogusVideo);

        var result = (Ok<VideoReadDto>) await VideoEndpoints.GetVideo(mockVideoRepository.Object, mapper, bogusVideo.Id);

        Assert.Equal(200, result.StatusCode);

        var resultValue = Assert.IsAssignableFrom<VideoReadDto>(result.Value);

        Assert.Equal(bogusVideo.Id, resultValue.Id);
    }

    [Fact]
    public async Task GetVide_WithNoKnownId_ReturnsNotFound()
    {
        mockVideoRepository.Setup(repository => repository.GetVideoByIdAsync(1));

        var result = (NotFound) await VideoEndpoints.GetVideo(mockVideoRepository.Object, mapper, 1);

        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task CreatVideo_ReturnsCreatedVideo()
    {
        var bogusCreatVideo = VideoCreatDto.BobusVideoCreateDto.Generate();
        var bogusVideo = VideoModel.BogusVideoModel.Generate();
        var sequence = new MockSequence();

        mockVideoRepository.InSequence(sequence).Setup(repository => repository.CreateVideoAsync(bogusVideo)).Verifiable();
        mockVideoRepository.InSequence(sequence).Setup(repository => repository.SaveChangesAsync()).Verifiable();

        var result = (Created<VideoReadDto>)await VideoEndpoints.CreateVideo(mockVideoRepository.Object, mapper, bogusCreatVideo);

        Assert.Equal(201, result.StatusCode);
        mockVideoRepository.Verify();
    }

    [Fact]
    public async Task UpdateVideo_WithId_ReturnsNoContent()
    {
        var bogusUpdateVideo = VideoUpdateDto.BogusVideoUpdateDto.Generate();
        var bogusVideo = VideoModel.BogusVideoModel.Generate();
        var sequence = new MockSequence();

        mockVideoRepository.InSequence(sequence).Setup(repository => repository.GetVideoByIdAsync(bogusVideo.Id)).ReturnsAsync(bogusVideo);
        mockVideoRepository.InSequence(sequence).Setup(repository => repository.SaveChangesAsync()).Verifiable();

        var result = (NoContent)await VideoEndpoints.UpdateVideo(mockVideoRepository.Object, mapper, bogusUpdateVideo, bogusVideo.Id);

        Assert.Equal(204, result.StatusCode);
        mockVideoRepository.Verify();
    }

    [Fact]
    public async Task UpdateVide_WithNoKnownId_ReturnNotFound()
    {
        var bogusUpdateVideo = VideoUpdateDto.BogusVideoUpdateDto.Generate();

        mockVideoRepository.Setup(repository => repository.GetVideoByIdAsync(1));

        var result = (NotFound)await VideoEndpoints.UpdateVideo(mockVideoRepository.Object, mapper, bogusUpdateVideo, 1);

        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task DeleteVide_WithId_ReturnOkWithVideo()
    {
        var bogusVideo = VideoModel.BogusVideoModel.Generate();
        var sequence = new MockSequence();

        mockVideoRepository.InSequence(sequence).Setup(repository => repository.GetVideoByIdAsync(bogusVideo.Id)).ReturnsAsync(bogusVideo);
        mockVideoRepository.InSequence(sequence).Setup(repository => repository.DeleteVideo(bogusVideo)).Verifiable();
        mockVideoRepository.InSequence(sequence).Setup(repository => repository.SaveChangesAsync()).Verifiable();

        var result = (Ok<VideoModel>)await VideoEndpoints.DeleteVideo(mockVideoRepository.Object, bogusVideo.Id);

        Assert.Equal(200, result.StatusCode);
        mockVideoRepository.Verify();
    }

    [Fact]
    public async Task DeleteVide_WithNoKnownId_ReturnNotFound()
    {
        mockVideoRepository.Setup(repository => repository.GetVideoByIdAsync(1));

        var result = (NotFound)await VideoEndpoints.DeleteVideo(mockVideoRepository.Object, 1);

        Assert.Equal(404, result.StatusCode);
    }
}
