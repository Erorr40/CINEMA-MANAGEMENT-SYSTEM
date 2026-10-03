using System.ComponentModel.DataAnnotations;

namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.MovieDTOs
{
    public class CreateMovieDTO
    {
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
        public decimal TicketPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableSeats { get; set; }

        [Required]
        public int GenreId { get; set; }
    }
}

