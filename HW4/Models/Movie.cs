namespace HW4.Models
{   
    public class Movie
    {   
        // Not sure if any properties need to be nullable yet, 
        // or if TMDB will return a default placeholder value for any missing properties
        public int Id { get; set; }
        public string PosterPath { get; set; }
        public string Title { get; set; }
        public string ReleaseDate { get; set; }
        public string Overview { get; set; }

    }
}