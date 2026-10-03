namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.MovieDTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public double Rating { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableSeats { get; set; }
        public int GenreId { get; set; }
    }
}

