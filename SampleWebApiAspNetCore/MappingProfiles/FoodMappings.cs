using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class AnimeMappings : Profile
    {
        public AnimeMappings()
        {
            CreateMap<AnimeEntity, AnimeDto>().ReverseMap();
            CreateMap<AnimeEntity, AnimeUpdateDto>().ReverseMap();
            CreateMap<AnimeEntity, AnimeCreateDto>().ReverseMap();
        }
    }
}
