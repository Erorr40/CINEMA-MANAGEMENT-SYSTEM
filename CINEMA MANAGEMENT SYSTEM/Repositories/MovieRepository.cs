using Microsoft.EntityFrameworkCore;
using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(CinemaDbContext context) : base(context)
        {
        }

        public IEnumerable<Movie> SearchByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return GetAll();
            }

            return _context.Movies
                .Where(m => m.Title.ToLower().Contains(title.Trim().ToLower()))
                .ToList();
        }

        public IEnumerable<Movie> GetMoviesWithRatingGreaterThan(double rating)
        {
            return _context.Movies
                .Where(m => m.Rating > rating)
                .ToList();
        }

        public IEnumerable<Movie> GetMoviesWithPriceBetween(decimal min, decimal max)
        {
            return _context.Movies
                .Where(m => m.TicketPrice >= min && m.TicketPrice <= max)
                .ToList();
        }

        public IEnumerable<Movie> GetAvailableMovies()
        {
            return _context.Movies
                .Where(m => m.AvailableSeats > 0)
                .ToList();
        }

        public IEnumerable<Movie> GetTopRatedMovies(int count = 5)
        {
            return _context.Movies
                .OrderByDescending(m => m.Rating)
                .Take(count)
                .ToList();
        }

        public Movie? GetLatestMovie()
        {
            return _context.Movies
                .OrderByDescending(m => m.ReleaseYear)
                .FirstOrDefault();
        }

        public Movie? GetMostBookedMovie()
        {
            return _context.Movies
                .OrderByDescending(m => m.Bookings.Count)
                .FirstOrDefault();
        }

        public int GetTotalCount()
        {
            return _context.Movies.Count();
        }

        public decimal GetAveragePrice()
        {
            if (!_context.Movies.Any())
            {
                return 0m;
            }

            return _context.Movies.Average(m => m.TicketPrice);
        }

        public IEnumerable<Movie> GetMoviesWithGenreDetails()
        {
            return _context.Movies
                .Include(m => m.Genre)
                .ToList();
        }

        public Movie? GetMovieWithGenreDetailsById(int id)
        {
            return _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Movie> GetPagedMovies(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return _context.Movies
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public bool HasBookings(int movieId)
        {
            return _context.Bookings.Any(b => b.MovieId == movieId);
        }

        public bool TitleExists(string title, int? excludeId = null)
        {
            var query = _context.Movies.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }
            return query.Any(m => m.Title.ToLower() == title.Trim().ToLower());
        }
    }
}

