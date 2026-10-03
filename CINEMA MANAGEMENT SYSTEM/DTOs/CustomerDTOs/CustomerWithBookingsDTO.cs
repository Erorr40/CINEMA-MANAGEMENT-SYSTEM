using CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs;

namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.CustomerDTOs
{
    public class CustomerWithBookingsDTO : CustomerDTO
    {
        public List<BookingDTO> Bookings { get; set; } = new List<BookingDTO>();
    }
}

