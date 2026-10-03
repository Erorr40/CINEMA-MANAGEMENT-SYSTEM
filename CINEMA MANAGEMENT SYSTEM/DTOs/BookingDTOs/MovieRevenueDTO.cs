namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs
{
    public class MovieRevenueDTO
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

