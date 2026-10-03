namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenreRepository Genres { get; }
        IMovieRepository Movies { get; }
        ICustomerRepository Customers { get; }
        IBookingRepository Bookings { get; }
        int Complete();
    }
}

