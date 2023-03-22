using CommentServiceAPI;
using CommentServiceAPI.Data.Repositories;
using CommentServiceAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceTests;
public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> mockCommentRepository = new Mock<ICommentRepository>();

    [Fact]
    public async Task GetCommentsOfContent_ReturnsOkWithComments ()
    {
        var bogusComments = CommentModel.BogusCommentModel.Generate(3);

        mockCommentRepository.Setup(repository => 
            repository.GetCommentsOfContentAsync(It.IsAny<string>())).ReturnsAsync(bogusComments);

        var result = (Ok<List<CommentModel>>)await CommentEndpoints.GetCommentsOfContent(mockCommentRepository.Object, It.IsAny<string>());

        Assert.Equal(200, result.StatusCode);

        var list = Assert.IsAssignableFrom<List<CommentModel>>(result.Value);

        Assert.Equal(3, list.ToList().Count);
    }

    [Fact]
    public async Task CreateComment_ReturnsCreatedComment()
    {
        var bogusComment = CommentModel.BogusCommentModel.Generate();

        mockCommentRepository.Setup(repository => repository.CreateCommentAsync(bogusComment)).Verifiable(); ;

        var result = (Created<CommentModel>)await CommentEndpoints.CreateComment(mockCommentRepository.Object, bogusComment);

        Assert.Equal(201, result.StatusCode);
        mockCommentRepository.Verify();
    }

    [Fact]
    public async Task UpdateComment_WithNoKnownId_ReturnsNotFound()
    {
        var bogusComment = CommentModel.BogusCommentModel.Generate();

        mockCommentRepository.Setup(repository => repository.GetCommentByIdAsync(It.IsAny<string>()));

        var result = (NotFound)await CommentEndpoints.UpdateComment(mockCommentRepository.Object, bogusComment, It.IsAny<string>());

        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task UpdateComment_WithId_ReturnsNoContent()
    {
        var bogusComment = CommentModel.BogusCommentModel.Generate();
        var sequence = new MockSequence();

        mockCommentRepository.InSequence(sequence).Setup(repository => repository.GetCommentByIdAsync(It.IsAny<string>())).ReturnsAsync(bogusComment);
        mockCommentRepository.InSequence(sequence).Setup(repository => repository.UpdateCommentAsync(bogusComment, It.IsAny<string>())).Verifiable();

        var result = (NoContent)await CommentEndpoints.UpdateComment(mockCommentRepository.Object, bogusComment, It.IsAny<string>());

        Assert.Equal(204, result.StatusCode);
        mockCommentRepository.Verify();
    }

    [Fact]
    public async Task DeleteComment_WithNoKnownId_ReturnsNotFound()
    {
        var bogusComment = CommentModel.BogusCommentModel.Generate();

        mockCommentRepository.Setup(repository => repository.GetCommentByIdAsync(It.IsAny<string>()));

        var result = (NotFound)await CommentEndpoints.DeleteComment(mockCommentRepository.Object, It.IsAny<string>());

        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task DeleteComment_WithId_OkWithComment()
    {
        var bogusComment = CommentModel.BogusCommentModel.Generate();
        var sequence = new MockSequence();

        mockCommentRepository.InSequence(sequence).Setup(repository => repository.GetCommentByIdAsync(It.IsAny<string>())).ReturnsAsync(bogusComment);
        mockCommentRepository.InSequence(sequence).Setup(repository => repository.DeletCommentAsync(It.IsAny<string>())).Verifiable();

        var result = (Ok<CommentModel>)await CommentEndpoints.DeleteComment(mockCommentRepository.Object, It.IsAny<string>());

        Assert.Equal(200, result.StatusCode);
        mockCommentRepository.Verify();
    }
    
}
