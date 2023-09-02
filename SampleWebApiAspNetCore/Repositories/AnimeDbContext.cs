using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class AnimeDbContext : DbContext
    {
        public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
            : base(options)
        {
        }

        public DbSet<AnimeEntity> AnimeItems { get; set; } = null!;
    }
}
