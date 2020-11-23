using CacheWars.Domain.Movies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CacheWars.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesControllers : ControllerBase
    {
        private readonly IMovieService movieService;

        public MoviesControllers(IMovieService movieService)
        {
            this.movieService = movieService;
        }
        [HttpGet]
        public Task<StarwarsMovie> Index(int number)
        {
            return movieService.GetMovie(number);
        }
    }
}
