using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AnimeController : ControllerBase
    {
        private readonly IAnimeRepository _animeRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<AnimeController> _linkService;

        public AnimeController(
            IAnimeRepository animeRepository,
            IMapper mapper,
            ILinkService<AnimeController> linkService)
        {
            _animeRepository = animeRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllAnime))]
        public ActionResult GetAllAnime(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<AnimeEntity> animeItems = _animeRepository.GetAll(queryParameters).ToList();

            var allItemCount = _animeRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = animeItems.Select(x => _linkService.ExpandSingleAnimeItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleAnime))]
        public ActionResult GetSingleAnime(ApiVersion version, int id)
        {
            AnimeEntity animeItem = _animeRepository.GetSingle(id);

            if (animeItem == null)
            {
                return NotFound();
            }

            AnimeDto item = _mapper.Map<AnimeDto>(animeItem);

            return Ok(_linkService.ExpandSingleAnimeItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddAnime))]
        public ActionResult<AnimeDto> AddAnime(ApiVersion version, [FromBody] AnimeCreateDto animeCreateDto)
        {
            if (animeCreateDto == null)
            {
                return BadRequest();
            }

            AnimeEntity toAdd = _mapper.Map<AnimeEntity>(animeCreateDto);

            _animeRepository.Add(toAdd);

            if (!_animeRepository.Save())
            {
                throw new Exception("Creating a animeitem failed on save.");
            }

            AnimeEntity newAnimeItem = _animeRepository.GetSingle(toAdd.Id);
            AnimeDto animeDto = _mapper.Map<AnimeDto>(newAnimeItem);

            return CreatedAtRoute(nameof(GetSingleAnime),
                new { version = version.ToString(), id = newAnimeItem.Id },
                _linkService.ExpandSingleAnimeItem(animeDto, animeDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateAnime))]
        public ActionResult<AnimeDto> PartiallyUpdateAnime(ApiVersion version, int id, [FromBody] JsonPatchDocument<AnimeUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            AnimeEntity existingEntity = _animeRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            AnimeUpdateDto animeUpdateDto = _mapper.Map<AnimeUpdateDto>(existingEntity);
            patchDoc.ApplyTo(animeUpdateDto);

            TryValidateModel(animeUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(animeUpdateDto, existingEntity);
            AnimeEntity updated = _animeRepository.Update(id, existingEntity);

            if (!_animeRepository.Save())
            {
                throw new Exception("Updating a anime failed on save.");
            }

            AnimeDto animeDto = _mapper.Map<AnimeDto>(updated);

            return Ok(_linkService.ExpandSingleAnimeItem(animeDto, animeDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveAnime))]
        public ActionResult RemoveAnime(int id)
        {
            AnimeEntity animeItem = _animeRepository.GetSingle(id);

            if (animeItem == null)
            {
                return NotFound();
            }

            _animeRepository.Delete(id);

            if (!_animeRepository.Save())
            {
                throw new Exception("Deleting a anime failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateAnime))]
        public ActionResult<AnimeDto> UpdateAnime(ApiVersion version, int id, [FromBody] AnimeUpdateDto animeUpdateDto)
        {
            if (animeUpdateDto == null)
            {
                return BadRequest();
            }

            var existingAnimeItem = _animeRepository.GetSingle(id);

            if (existingAnimeItem == null)
            {
                return NotFound();
            }

            _mapper.Map(animeUpdateDto, existingAnimeItem);

            _animeRepository.Update(id, existingAnimeItem);

            if (!_animeRepository.Save())
            {
                throw new Exception("Updating a anime failed on save.");
            }

            AnimeDto animeDto = _mapper.Map<AnimeDto>(existingAnimeItem);

            return Ok(_linkService.ExpandSingleAnimeItem(animeDto, animeDto.Id, version));
        }

        [HttpGet("GetRandomAnime", Name = nameof(GetRandomAnime))]
        public ActionResult GetRandomAnime()
        {
            ICollection<AnimeEntity> animeItems = _animeRepository.GetRandomMeal();

            IEnumerable<AnimeDto> dtos = animeItems.Select(x => _mapper.Map<AnimeDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomAnime), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
