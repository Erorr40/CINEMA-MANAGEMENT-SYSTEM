using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaDbContext _context;

        public IGenreRepository Genres { get; private set; }
        public IMovieRepository Movies { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IBookingRepository Bookings { get; private set; }

        public UnitOfWork(CinemaDbContext context)
        {
            _context = context;
            Genres = new GenreRepository(_context);
            Movies = new MovieRepository(_context);
            Customers = new CustomerRepository(_context);
            Bookings = new BookingRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

