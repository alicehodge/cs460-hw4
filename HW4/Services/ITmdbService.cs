using HW4.Models;

namespace HW4.Services
{
    public interface ITmdbService
    {
        Task<IEnumerable<Movie>> SearchMovie(string title);

        Task<MovieDetail> GetMovieDetail(int id);

        Task<MovieCast> GetMovieCast(int id);

        string FormatReleaseDate(string releaseDate);

        string FormatRuntime(int runtime);
    }

}