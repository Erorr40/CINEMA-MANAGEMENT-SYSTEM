using System.ComponentModel.DataAnnotations;

namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs
{
    public class CreateBookingDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Range(1, int.MaxValue)]
        public int NumberOfTickets { get; set; }
    }
}

