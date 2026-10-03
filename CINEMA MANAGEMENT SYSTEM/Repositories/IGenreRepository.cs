using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        IEnumerable<Genre> GetGenresWithMovies();
        Genre? GetGenreWithMoviesById(int id);
        bool HasMovies(int genreId);
        bool NameExists(string name, int? excludeId = null);
    }
}

