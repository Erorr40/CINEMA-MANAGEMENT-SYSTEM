using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        IEnumerable<Movie> SearchByTitle(string title);
        IEnumerable<Movie> GetMoviesWithRatingGreaterThan(double rating);
        IEnumerable<Movie> GetMoviesWithPriceBetween(decimal min, decimal max);
        IEnumerable<Movie> GetAvailableMovies();
        IEnumerable<Movie> GetTopRatedMovies(int count = 5);
        Movie? GetLatestMovie();
        Movie? GetMostBookedMovie();
        int GetTotalCount();
        decimal GetAveragePrice();
        IEnumerable<Movie> GetMoviesWithGenreDetails();
        Movie? GetMovieWithGenreDetailsById(int id);
        IEnumerable<Movie> GetPagedMovies(int pageNumber, int pageSize);
        bool HasBookings(int movieId);
        bool TitleExists(string title, int? excludeId = null);
    }
}

