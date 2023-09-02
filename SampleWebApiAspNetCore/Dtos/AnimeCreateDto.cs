using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class AnimeCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public int Episodes { get; set; }
        public DateTime Aired { get; set; }
    }
}
