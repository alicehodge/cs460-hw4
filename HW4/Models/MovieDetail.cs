namespace HW4.Models
{
    public class MovieDetail
    {
        public string Title { get; set; }
        public string BackdropPath { get; set; } // background img, diff than poster path
        public string ReleaseDate { get; set; }
        public List<string> Genres { get; set; }
        public string Runtime { get; set; }
        public Int64 Revenue { get; set; }
        public double Popularity { get; set; }
        public string Overview { get; set; }

    }
}