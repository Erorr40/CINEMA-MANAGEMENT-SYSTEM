using Microsoft.AspNetCore.Mvc;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.BookingDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;
using CINEMA_MANAGEMENT_SYSTEM.Repositories;

namespace CINEMA_MANAGEMENT_SYSTEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var bookings = _unitOfWork.Bookings.GetAll();
            var dtos = bookings.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("details")]
        public IActionResult GetDetails()
        {
            var bookings = _unitOfWork.Bookings.GetBookingDetails();
            var dtos = bookings.Select(b => new BookingDetailsDTO
            {
                Id = b.Id,
                CustomerName = b.Customer?.FullName ?? string.Empty,
                MovieTitle = b.Movie?.Title ?? string.Empty,
                GenreName = b.Movie?.Genre?.Name ?? string.Empty,
                BookingDate = b.BookingDate,
                NumberOfTickets = b.NumberOfTickets,
                TotalPrice = b.TotalPrice
            });

            return Ok(dtos);
        }

        [HttpGet("revenue")]
        public IActionResult GetRevenueReport()
        {
            var report = _unitOfWork.Bookings.GetMovieRevenueReport();
            return Ok(report);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var booking = _unitOfWork.Bookings.GetById(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            return Ok(MapToDto(booking));
        }

        [HttpPost("book")]
        public IActionResult Book([FromBody] CreateBookingDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _unitOfWork.Customers.GetById(dto.CustomerId);
            if (customer == null)
            {
                return BadRequest($"Customer with ID {dto.CustomerId} does not exist.");
            }

            var movie = _unitOfWork.Movies.GetById(dto.MovieId);
            if (movie == null)
            {
                return BadRequest($"Movie with ID {dto.MovieId} does not exist.");
            }

            if (dto.NumberOfTickets > movie.AvailableSeats)
            {
                return BadRequest($"Not enough available seats. Requested: {dto.NumberOfTickets}, Available: {movie.AvailableSeats}.");
            }

            movie.AvailableSeats -= dto.NumberOfTickets;
            if (movie.AvailableSeats < 0)
            {
                return BadRequest("Available seats cannot become negative.");
            }

            var totalPrice = dto.NumberOfTickets * movie.TicketPrice;
            var bookingDate = DateTime.Now;

            var booking = new Booking
            {
                CustomerId = dto.CustomerId,
                MovieId = dto.MovieId,
                NumberOfTickets = dto.NumberOfTickets,
                TotalPrice = totalPrice,
                BookingDate = bookingDate
            };

            _unitOfWork.Bookings.Add(booking);
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, MapToDto(booking));
        }

        [HttpPut("cancel/{id:int}")]
        public IActionResult Cancel(int id)
        {
            var booking = _unitOfWork.Bookings.GetById(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            var movie = _unitOfWork.Movies.GetById(booking.MovieId);
            if (movie != null)
            {
                movie.AvailableSeats += booking.NumberOfTickets;
                _unitOfWork.Movies.Update(movie);
            }

            _unitOfWork.Bookings.Delete(booking);
            _unitOfWork.Complete();

            return Ok("Booking cancelled successfully.");
        }

        private static BookingDTO MapToDto(Booking b) => new()
        {
            Id = b.Id,
            BookingDate = b.BookingDate,
            NumberOfTickets = b.NumberOfTickets,
            TotalPrice = b.TotalPrice,
            CustomerId = b.CustomerId,
            MovieId = b.MovieId
        };
    }
}

