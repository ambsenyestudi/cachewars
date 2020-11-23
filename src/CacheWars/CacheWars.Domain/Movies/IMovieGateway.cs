using System.Threading.Tasks;

namespace CacheWars.Domain.Movies
{
    public interface IMovieGateway
    {
        Task<StarwarsMovieResults> GetMovieResults(int number);
    }
}
