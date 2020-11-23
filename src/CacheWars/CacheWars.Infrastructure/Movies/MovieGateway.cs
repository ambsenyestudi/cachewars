using CacheWars.Domain.Movies;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CacheWars.Infrastructure.Movies
{
    public class MovieGateway : IMovieGateway
    {
        private readonly HttpClient client;
        private readonly MoviesSettings moviesSettings;

        public MovieGateway(HttpClient client, IOptions<MoviesSettings> moviesSettings)
        {
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.26.5");
            this.client = client;
            this.moviesSettings = moviesSettings.Value;
        }
        public async Task<StarwarsMovieResults> GetMovieResults(int number)
        {
            var url = $"{moviesSettings.Url}/";
            var response = await client.GetAsync(url);
            if (response.Content is object &&
               response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(contentStream))
                {
                    var json = streamReader.ReadToEnd();
                    var results = JsonConvert.DeserializeObject<StarwarsMovieResults>(json);
                    return results;
                }
            }
            return null;
        }
    }
}
