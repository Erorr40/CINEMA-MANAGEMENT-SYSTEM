using Microsoft.EntityFrameworkCore;
using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(CinemaDbContext context) : base(context)
        {
        }

        public IEnumerable<Genre> GetGenresWithMovies()
        {
            return _context.Genres
                .Include(g => g.Movies)
                .ToList();
        }

        public Genre? GetGenreWithMoviesById(int id)
        {
            return _context.Genres
                .Include(g => g.Movies)
                .FirstOrDefault(g => g.Id == id);
        }

        public bool HasMovies(int genreId)
        {
            return _context.Movies.Any(m => m.GenreId == genreId);
        }

        public bool NameExists(string name, int? excludeId = null)
        {
            var query = _context.Genres.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(g => g.Id != excludeId.Value);
            }
            return query.Any(g => g.Name.ToLower() == name.Trim().ToLower());
        }
    }
}

