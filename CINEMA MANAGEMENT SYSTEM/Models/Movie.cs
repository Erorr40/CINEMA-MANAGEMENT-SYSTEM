using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CINEMA_MANAGEMENT_SYSTEM.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Director { get; set; } = string.Empty;

        [Range(30, 300)]
        public int Duration { get; set; }

        [Range(1950, 2026)]
        public int ReleaseYear { get; set; }

        [Range(0, 10)]
        public double Rating { get; set; }

        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TicketPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableSeats { get; set; }

        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
