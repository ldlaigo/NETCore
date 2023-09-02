using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IAnimeRepository
    {
        AnimeEntity GetSingle(int id);
        void Add(AnimeEntity item);
        void Delete(int id);
        AnimeEntity Update(int id, AnimeEntity item);
        IQueryable<AnimeEntity> GetAll(QueryParameters queryParameters);
        ICollection<AnimeEntity> GetRandomMeal();
        int Count();
        bool Save();
    }
}
