using Microsoft.AspNetCore.Mvc;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.MovieDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;
using CINEMA_MANAGEMENT_SYSTEM.Repositories;

namespace CINEMA_MANAGEMENT_SYSTEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MoviesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var movies = _unitOfWork.Movies.GetAll();
            var dtos = movies.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? title)
        {
            var movies = _unitOfWork.Movies.SearchByTitle(title ?? string.Empty);
            var dtos = movies.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("rating-greater-than/{rating:double}")]
        public IActionResult GetByRatingGreaterThan(double rating)
        {
            var movies = _unitOfWork.Movies.GetMoviesWithRatingGreaterThan(rating);
            var dtos = movies.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("price-between")]
        public IActionResult GetByPriceBetween([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var movies = _unitOfWork.Movies.GetMoviesWithPriceBetween(min, max);
            var dtos = movies.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("available")]
        public IActionResult GetAvailable()
        {
            var movies = _unitOfWork.Movies.GetAvailableMovies();
            var dtos = movies.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("top-rated")]
        public IActionResult GetTopRated()
        {
            var movies = _unitOfWork.Movies.GetTopRatedMovies(5);
            var dtos = movies.Select(MapToDto);
            return Ok(dtos);
        }

        [HttpGet("latest")]
        public IActionResult GetLatest()
        {
            var movie = _unitOfWork.Movies.GetLatestMovie();
            if (movie == null)
            {
                return NotFound("No movies found.");
            }

            return Ok(MapToDto(movie));
        }

        [HttpGet("most-booked")]
        public IActionResult GetMostBooked()
        {
            var movie = _unitOfWork.Movies.GetMostBookedMovie();
            if (movie == null)
            {
                return NotFound("No movies found.");
            }

            return Ok(MapToDto(movie));
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            var count = _unitOfWork.Movies.GetTotalCount();
            return Ok(new { totalMovies = count });
        }

        [HttpGet("average-price")]
        public IActionResult GetAveragePrice()
        {
            var averagePrice = _unitOfWork.Movies.GetAveragePrice();
            return Ok(new { averagePrice = Math.Round(averagePrice, 2) });
        }

        [HttpGet("genre-details")]
        public IActionResult GetWithGenreDetails()
        {
            var movies = _unitOfWork.Movies.GetMoviesWithGenreDetails();
            var dtos = movies.Select(m => new MovieWithGenreDTO
            {
                Id = m.Id,
                Title = m.Title,
                Director = m.Director,
                Duration = m.Duration,
                ReleaseYear = m.ReleaseYear,
                Rating = m.Rating,
                TicketPrice = m.TicketPrice,
                AvailableSeats = m.AvailableSeats,
                GenreId = m.GenreId,
                GenreName = m.Genre?.Name ?? string.Empty
            });

            return Ok(dtos);
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var movies = _unitOfWork.Movies.GetPagedMovies(pageNumber, pageSize);
            return Ok(movies.Select(MapToDto));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var movie = _unitOfWork.Movies.GetById(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }

            return Ok(MapToDto(movie));
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateMovieDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_unitOfWork.Movies.TitleExists(dto.Title))
            {
                return BadRequest("Movie title already exists.");
            }

            var genre = _unitOfWork.Genres.GetById(dto.GenreId);
            if (genre == null)
            {
                return BadRequest($"Genre with ID {dto.GenreId} does not exist.");
            }

            var movie = new Movie
            {
                Title = dto.Title.Trim(),
                Director = dto.Director.Trim(),
                Duration = dto.Duration,
                ReleaseYear = dto.ReleaseYear,
                Rating = dto.Rating,
                TicketPrice = dto.TicketPrice,
                AvailableSeats = dto.AvailableSeats,
                GenreId = dto.GenreId
            };

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, MapToDto(movie));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateMovieDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = _unitOfWork.Movies.GetById(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }

            if (_unitOfWork.Movies.TitleExists(dto.Title, id))
            {
                return BadRequest("Movie title already exists.");
            }

            var genre = _unitOfWork.Genres.GetById(dto.GenreId);
            if (genre == null)
            {
                return BadRequest($"Genre with ID {dto.GenreId} does not exist.");
            }

            if (movie.TicketPrice != dto.TicketPrice && _unitOfWork.Movies.HasBookings(id))
            {
                return BadRequest("Ticket price cannot be updated because bookings already exist for this movie.");
            }

            movie.Title = dto.Title.Trim();
            movie.Director = dto.Director.Trim();
            movie.Duration = dto.Duration;
            movie.ReleaseYear = dto.ReleaseYear;
            movie.Rating = dto.Rating;
            movie.TicketPrice = dto.TicketPrice;
            movie.AvailableSeats = dto.AvailableSeats;
            movie.GenreId = dto.GenreId;

            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Complete();

            return Ok(MapToDto(movie));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var movie = _unitOfWork.Movies.GetById(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }

            if (_unitOfWork.Movies.HasBookings(id))
            {
                return BadRequest("Cannot delete movie because bookings exist for it.");
            }

            _unitOfWork.Movies.Delete(movie);
            _unitOfWork.Complete();

            return Ok("Movie deleted successfully.");
        }

        private static MovieDTO MapToDto(Movie m) => new()
        {
            Id = m.Id,
            Title = m.Title,
            Director = m.Director,
            Duration = m.Duration,
            ReleaseYear = m.ReleaseYear,
            Rating = m.Rating,
            TicketPrice = m.TicketPrice,
            AvailableSeats = m.AvailableSeats,
            GenreId = m.GenreId
        };
    }
}

