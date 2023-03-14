using Microsoft.EntityFrameworkCore;
using VideoServiceAPI.Models;

namespace VideoServiceAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<VideoModel> Videos { get; set; }
}
