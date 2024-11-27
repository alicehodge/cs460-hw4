using HW4.Models;
using System.Globalization;
using System.Text.Json;

namespace HW4.Services
{
    // internal classes for deserialization go here
    internal class MovieResult
    {   
        public int Id { get; set; }
        public string Poster_Path { get; set; }
        public string Title { get; set; }
        public string Release_Date { get; set; }
        public string Overview { get; set; }

    }
    internal class TmdbSearchMovieResponse
    {
        public int Page { get; set; }
        public int TotalResults { get; set; }
        public IEnumerable<MovieResult> Results { get; set; }
    }

    internal class Genre
    {
        public string Name { get; set; }
    }

    internal class TmdbGetMovieDetailResponse
    {
        public string Title { get; set; }
        public string Backdrop_Path { get; set; }
        public string Release_Date { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public int Runtime { get; set; }
        public Int64 Revenue { get; set; }
        public double Popularity { get; set; }
        public string Overview { get; set; }
    }

    internal class TmdbGetMovieCastResponse
    {
        public IEnumerable<Role> Cast { get; set; }
    }

    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TmdbService> _logger;

        public TmdbService(HttpClient httpClient, ILogger<TmdbService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Movie>> SearchMovie(string title)
        {
            // endpoint: https://api.themoviedb.org/3/search/movie
            string endpoint = $"search/movie?query={title}";
            _logger.LogInformation($"Calling TMDB API at {endpoint}");

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            string responseBody;
            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response from TMDB API: {responseBody}");
            }
            else
            {
                _logger.LogError($"Error calling TMDB API: {response.StatusCode}");
                throw new Exception($"Error calling TMDB API: {response.StatusCode}"); // change this to a suitably typed exception later
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            TmdbSearchMovieResponse movies = JsonSerializer.Deserialize<TmdbSearchMovieResponse>(responseBody, options);
            return movies.Results.Select(m => new Movie
            {
                Id = m.Id,
                PosterPath = m.Poster_Path == null ? "https://placehold.co/500x750" : "https://image.tmdb.org/t/p/w500" + m.Poster_Path,
                Title = m.Title,
                ReleaseDate = FormatReleaseDate(m.Release_Date),
                Overview = string.IsNullOrEmpty(m.Overview) ? "No overview available" : m.Overview,
            });
        }

        public async Task<MovieDetail> GetMovieDetail(int id)
        {
            // endpoint: https://api.themoviedb.org/3/movie/{movie_id}
            string endpoint = $"movie/{id}";
            _logger.LogInformation($"Calling TMDB API at {endpoint}");

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            string responseBody;
            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response from TMDB API: {responseBody}");
            }
            else
            {
                _logger.LogError($"Error calling TMDB API: {response.StatusCode}");
                throw new Exception($"Error calling TMDB API: {response.StatusCode}");
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            TmdbGetMovieDetailResponse movie = JsonSerializer.Deserialize<TmdbGetMovieDetailResponse>(responseBody, options);

            return new MovieDetail
            {
                    Title = movie.Title,
                    BackdropPath = movie.Backdrop_Path == null ? "https://placehold.co/500x750" : "https://image.tmdb.org/t/p/w780" + movie.Backdrop_Path,
                    ReleaseDate = FormatReleaseDate(movie.Release_Date),
                    Genres = movie.Genres.Select(g => g.Name).ToList(),
                    Runtime = FormatRuntime(movie.Runtime),
                    Revenue = movie.Revenue,
                    Popularity = movie.Popularity,
                    Overview = string.IsNullOrEmpty(movie.Overview) ? "No overview available" : movie.Overview,
            };
        }

        public async Task<MovieCast> GetMovieCast(int id)
        {
            // endpoint: https://api.themoviedb.org/3/movie/{movie_id}/credits
            string endpoint = $"movie/{id}/credits";
            _logger.LogInformation($"Calling TMDB API at {endpoint}");

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            string responseBody;
            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response from TMDB API: {responseBody}");
            }
            else
            {
                _logger.LogError($"Error calling TMDB API: {response.StatusCode}");
                throw new Exception($"Error calling TMDB API: {response.StatusCode}");
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            TmdbGetMovieCastResponse movie = JsonSerializer.Deserialize<TmdbGetMovieCastResponse>(responseBody, options);

            return new MovieCast
            {
                Cast = movie.Cast.Select(c => new Role
                {
                    Name = c.Name,
                    Character = string.IsNullOrEmpty(c.Character) ? "Unknown character" : c.Character
                }).ToList()
            };
        }

        public string FormatReleaseDate(string releaseDate)
        {
            if (DateTime.TryParse(releaseDate, out DateTime date))
            {
                return date.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);
            }
            return "Unknown release date";
        }

        public string FormatRuntime(int runtime)
        {
            if (runtime < 60)
            {
                return $"{runtime} minutes";
            }
            int hours = runtime / 60;
            int minutes = runtime % 60;

            if (minutes == 0)
            {
                return hours == 1 ? $"{hours} hour" : $"{hours} hours";
            } else if (hours == 1) {
                return minutes == 1 ? $"{hours} hour {minutes} minute" : $"{hours} hour {minutes} minutes";
            } else {
                return minutes == 1 ? $"{hours} hours {minutes} minute" : $"{hours} hours {minutes} minutes";
            }
        }
    }
}