namespace SampleWebApiAspNetCore.Dtos
{
    public class AnimeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public int Episodes { get; set; }
        public DateTime Aired { get; set; }
    }
}
