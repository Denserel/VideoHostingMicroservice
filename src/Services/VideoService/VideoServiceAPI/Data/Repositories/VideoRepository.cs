using Microsoft.EntityFrameworkCore;
using VideoServiceAPI.Models;

namespace VideoServiceAPI.Data.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly ApplicationDbContext context;

    public VideoRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<VideoModel>> GetAllVideosAsync()
    {
        return await context.Videos.ToListAsync();
    }

    public async Task<VideoModel?> GetVideoByIdAsync(int id)
    {
        return await context.Videos.FindAsync(id);
    }

    public async Task CreateVideoAsync(VideoModel video)
    {
        if (video is null) throw new ArgumentNullException(nameof(video));

        await context.Videos.AddAsync(video);
    }

    public void DeleteVideo(VideoModel video)
    {
        if (video is null) throw new ArgumentNullException(nameof(video));

        context.Videos.Remove(video);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
