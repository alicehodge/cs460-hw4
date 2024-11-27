using HW4.Models;
using HW4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HW4.Controllers
{
    [Route("api/movie")]
    [ApiController]
    public class TmdbApiController : ControllerBase
    {
        private readonly ITmdbService _tmdbService;
        private readonly ILogger<TmdbApiController> _logger;

        public TmdbApiController(ITmdbService tmdbService, ILogger<TmdbApiController> logger)
        {
            _tmdbService = tmdbService;
            _logger = logger;
        }

        // GET: api/movie/search?title=foo
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(IEnumerable<Movie>))]
        public async Task<IActionResult> SearchMovieAsync(string title)
        {
            try 
            {
                IEnumerable<Movie> movies = await _tmdbService.SearchMovie(title);
                return Ok(movies);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error searching for movie");
                return StatusCode(500, new { message = e.Message });
            }
        }

        // GET: api/movie/123
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(MovieDetail))]
        public async Task<IActionResult> GetMovieDetailAsync(int id)
        {
            try
            {
                MovieDetail movie = await _tmdbService.GetMovieDetail(id);
                return Ok(movie);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting movie");
                return StatusCode(500, new { message = e.Message });
            }
        }

        // GET: api/movie/123/cast
        [HttpGet("{id}/cast")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(MovieCast))]
        public async Task<IActionResult> GetMovieCastAsync(int id)
        {
            try
            {
                MovieCast movie = await _tmdbService.GetMovieCast(id);
                return Ok(movie);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting movie");
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}