using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CINEMA_MANAGEMENT_SYSTEM.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Range(1, int.MaxValue)]
        public int NumberOfTickets { get; set; }

        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
