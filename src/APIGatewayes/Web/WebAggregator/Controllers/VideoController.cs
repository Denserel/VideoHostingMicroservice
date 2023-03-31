using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using WebAggregator.Models.Dtos;

namespace WebAggregator.Controllers;

[ApiController]
public class VideoController : Controller
{
    public VideoController()
    {
        
    }

    [HttpGet]
    public async Task<ActionResult<List<VideoDto>>> GetAllVideos()
    {
        throw new NotImplementedException();
    }

    [HttpGet("id")]
    public async Task<ActionResult<VideoDto>> GetVieoById(int id)
    {
        throw new NotImplementedException();
    }
}
