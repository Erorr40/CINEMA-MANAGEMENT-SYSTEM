using CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        IEnumerable<Booking> GetBookingDetails();
        Booking? GetBookingDetailsById(int id);
        IEnumerable<MovieRevenueDTO> GetMovieRevenueReport();
    }
}

