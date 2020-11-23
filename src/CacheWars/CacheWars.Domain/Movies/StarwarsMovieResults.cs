using System.Collections.Generic;

namespace CacheWars.Domain.Movies
{
    public class StarwarsMovieResults
    {
        public IEnumerable<StarwarsMovie> Results { get; set; }
        public StarwarsMovieResults()
        {
            Results = new List<StarwarsMovie>();
        }
    }
}
