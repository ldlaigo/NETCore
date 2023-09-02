using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(AnimeDbContext context)
        {
            context.AnimeItems.Add(new AnimeEntity() { Episodes = 24, Genre = "Comedy", Name = "SpyXFamily", Aired = DateTime.Now });
            context.AnimeItems.Add(new AnimeEntity() { Episodes = 24, Genre = "Mecha", Name = "Gundam Witch", Aired = DateTime.Now });
            context.AnimeItems.Add(new AnimeEntity() { Episodes = 12, Genre = "Slice-of-Life", Name = "Freiren", Aired = DateTime.Now });
            context.AnimeItems.Add(new AnimeEntity() { Episodes = 12, Genre = "Comedy", Name = "Zom100", Aired = DateTime.Now });

            context.SaveChanges();
        }
    }
}
