namespace EventsService.Models
{
    public class MovieEvent
    {
        public int movie_id { get; set; }
        public string title { get; set; }
        public string action { get; set; }
        public string? description { get; set; }
        public int user_id { get; set; }
        public float? rating { get; set; }
        public string[]? genres { get; set; }
    }
}
