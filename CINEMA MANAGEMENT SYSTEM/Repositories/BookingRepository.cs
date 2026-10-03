using Microsoft.EntityFrameworkCore;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;

namespace CINEMA_MANAGEMENT_SYSTEM.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(CinemaDbContext context) : base(context)
        {
        }

        public IEnumerable<Booking> GetBookingDetails()
        {
            return _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Movie)
                    .ThenInclude(m => m!.Genre)
                .ToList();
        }

        public Booking? GetBookingDetailsById(int id)
        {
            return _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Movie)
                    .ThenInclude(m => m!.Genre)
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<MovieRevenueDTO> GetMovieRevenueReport()
        {
            return _context.Movies
                .Select(m => new MovieRevenueDTO
                {
                    MovieId = m.Id,
                    MovieTitle = m.Title,
                    TotalTicketsSold = m.Bookings.Sum(b => (int?)b.NumberOfTickets) ?? 0,
                    TotalRevenue = m.Bookings.Sum(b => (decimal?)b.TotalPrice) ?? 0m
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToList();
        }
    }
}

