namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public int MovieId { get; set; }
    }
}

