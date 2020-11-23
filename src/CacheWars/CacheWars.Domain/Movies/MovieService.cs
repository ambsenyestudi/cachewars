using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CacheWars.Domain.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IMovieGateway movieGateway;
        private readonly ILogger<MovieService> logger;

        public MovieService(IMovieGateway movieGateway, ILogger<MovieService> logger)
        {
            this.movieGateway = movieGateway;
            this.logger = logger;
        }
        public async Task<StarwarsMovie> GetMovie(int number)
        {
            logger.LogInformation("Getting movie");
            var results = await movieGateway.GetMovieResults(number);
            return results.Results.FirstOrDefault(sm => sm.Episode_id == number);
        }
    }
}
