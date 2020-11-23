using CacheWars.Domain.Movies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CacheWars.Infrastructure.Movies
{
    public class MovieGateway : IMovieGateway
    {
        private readonly IMemoryCache cache;
        private readonly HttpClient client;
        private readonly MoviesSettings moviesSettings;
        private readonly MemoryCacheEntryOptions cacheEntryOptions;

        public MovieGateway(HttpClient client, IMemoryCache cache, IOptions<MoviesSettings> moviesSettings)//
        {
            this.moviesSettings = moviesSettings.Value;
            cacheEntryOptions = CreateCacheOptions(this.moviesSettings);
            this.cache = cache;
            this.client = client;
            
        }
        
        public async Task<StarwarsMovieResults> GetMovieResults(int number)
        {
            if(cache.TryGetValue(number, out StarwarsMovie starwarsMovie))
            {
                return new StarwarsMovieResults
                {
                    Results = new List<StarwarsMovie> { starwarsMovie }
                };
            }
            
            var movieResults = await GetMoviesFromBackend();
            if (movieResults.Results.Any())
            {
                CacheMovieResults(movieResults.Results);
            }
            return movieResults;
        }

        private async Task<StarwarsMovieResults> GetMoviesFromBackend()
        {
            var url = $"{moviesSettings.Url}/";

            await Task.Delay(5000);

            var response = await client.GetAsync(url);
            if (response.Content is object &&
               response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(contentStream))
                {
                    var json = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<StarwarsMovieResults>(json);
                }
            }
            return new StarwarsMovieResults ();
        }
        private void CacheMovieResults(IEnumerable<StarwarsMovie> starwarsMovieCollection)
        {
            foreach (var swMovie in starwarsMovieCollection)
            {
                cache.Set(swMovie.Episode_id, swMovie, cacheEntryOptions);
            }
        }

        private MemoryCacheEntryOptions CreateCacheOptions(MoviesSettings settings) =>
            CreateCacheOptionsFromTTL(settings.CacheMinutes);

        private MemoryCacheEntryOptions CreateCacheOptionsFromTTL(
            int cacheMinutes, 
            CacheItemPriority cacheItemPriority = CacheItemPriority.Normal) =>
            new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(cacheMinutes),
                Priority = cacheItemPriority
            };
    }
}
