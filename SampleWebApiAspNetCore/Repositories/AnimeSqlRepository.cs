using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class AnimeSqlRepository : IAnimeRepository
    {
        private readonly AnimeDbContext _animeDbContext;

        public AnimeSqlRepository(AnimeDbContext animeDbContext)
        {
            _animeDbContext = animeDbContext;
        }

        public AnimeEntity GetSingle(int id)
        {
            return _animeDbContext.AnimeItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(AnimeEntity item)
        {
            _animeDbContext.AnimeItems.Add(item);
        }

        public void Delete(int id)
        {
            AnimeEntity animeItem = GetSingle(id);
            _animeDbContext.AnimeItems.Remove(animeItem);
        }

        public AnimeEntity Update(int id, AnimeEntity item)
        {
            _animeDbContext.AnimeItems.Update(item);
            return item;
        }

        public IQueryable<AnimeEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<AnimeEntity> _allItems = _animeDbContext.AnimeItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Episodes.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _animeDbContext.AnimeItems.Count();
        }

        public bool Save()
        {
            return (_animeDbContext.SaveChanges() >= 0);
        }

        public ICollection<AnimeEntity> GetRandomMeal()
        {
            List<AnimeEntity> toReturn = new List<AnimeEntity>();

            toReturn.Add(GetRandomItem("Starter"));
            toReturn.Add(GetRandomItem("Main"));
            toReturn.Add(GetRandomItem("Dessert"));

            return toReturn;
        }

        private AnimeEntity GetRandomItem(string genre)
        {
            return _animeDbContext.AnimeItems
                .Where(x => x.Genre == genre)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
