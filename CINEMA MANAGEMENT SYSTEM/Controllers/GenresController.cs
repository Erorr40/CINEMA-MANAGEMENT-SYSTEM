using Microsoft.AspNetCore.Mvc;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.GenreDTOs;
using CINEMA_MANAGEMENT_SYSTEM.DTOs.MovieDTOs;
using CINEMA_MANAGEMENT_SYSTEM.Models;
using CINEMA_MANAGEMENT_SYSTEM.Repositories;

namespace CINEMA_MANAGEMENT_SYSTEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var genres = _unitOfWork.Genres.GetAll();
            var dtos = genres.Select(g => new GenreDTO
            {
                Id = g.Id,
                Name = g.Name
            });

            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var genre = _unitOfWork.Genres.GetById(id);
            if (genre == null)
            {
                return NotFound($"Genre with ID {id} not found.");
            }

            var dto = new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateGenreDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_unitOfWork.Genres.NameExists(dto.Name))
            {
                return BadRequest("Genre name already exists.");
            }

            var genre = new Genre
            {
                Name = dto.Name.Trim()
            };

            _unitOfWork.Genres.Add(genre);
            _unitOfWork.Complete();

            var resultDto = new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, resultDto);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateGenreDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = _unitOfWork.Genres.GetById(id);
            if (genre == null)
            {
                return NotFound($"Genre with ID {id} not found.");
            }

            if (_unitOfWork.Genres.NameExists(dto.Name, id))
            {
                return BadRequest("Genre name already exists.");
            }

            genre.Name = dto.Name.Trim();
            _unitOfWork.Genres.Update(genre);
            _unitOfWork.Complete();

            return Ok(new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name
            });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var genre = _unitOfWork.Genres.GetById(id);
            if (genre == null)
            {
                return NotFound($"Genre with ID {id} not found.");
            }

            if (_unitOfWork.Genres.HasMovies(id))
            {
                return BadRequest("Cannot delete genre because it contains movies.");
            }

            _unitOfWork.Genres.Delete(genre);
            _unitOfWork.Complete();

            return Ok("Genre deleted successfully.");
        }

        [HttpGet("movies")]
        public IActionResult GetGenresWithMovies()
        {
            var genres = _unitOfWork.Genres.GetGenresWithMovies();
            var dtos = genres.Select(g => new GenreWithMoviesDTO
            {
                Id = g.Id,
                Name = g.Name,
                Movies = g.Movies.Select(m => new MovieDTO
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
                }).ToList()
            });

            return Ok(dtos);
        }
    }
}

