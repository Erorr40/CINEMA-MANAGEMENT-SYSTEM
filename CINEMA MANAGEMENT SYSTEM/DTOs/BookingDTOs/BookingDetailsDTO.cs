namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs
{
    public class BookingDetailsDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string GenreName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

