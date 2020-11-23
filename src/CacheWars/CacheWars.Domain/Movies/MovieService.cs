using Microsoft.Extensions.Logging;
using System;
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
            var startTime = DateTime.Now;
            
            var results = await movieGateway.GetMovieResults(number);
            var ellapsedTime = DateTime.Now - startTime;
            logger.LogInformation("Ellapse time getting movie "+ ellapsedTime.TotalMilliseconds);
            return results.Results.FirstOrDefault(sm => sm.Episode_id == number);
        }
    }
}
