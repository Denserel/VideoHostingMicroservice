using VideoServiceAPI.Models;

namespace VideoServiceAPI.Data.Repositories;
public interface IVideoRepository
{
    Task CreateVideoAsync(VideoModel video);
    void DeleteVideo(VideoModel video);
    Task<IEnumerable<VideoModel>> GetAllVideosAsync();
    Task<VideoModel?> GetVideoByIdAsync(int id);
    Task SaveChangesAsync();
}