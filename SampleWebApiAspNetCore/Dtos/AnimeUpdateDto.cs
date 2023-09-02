
namespace SampleWebApiAspNetCore.Dtos
{
    public class AnimeUpdateDto
    {
        public string? Name { get; set; }
        public int Episodes { get; set; }
        public string? Genre { get; set; }
        public DateTime Aired { get; set; }
    }
}
