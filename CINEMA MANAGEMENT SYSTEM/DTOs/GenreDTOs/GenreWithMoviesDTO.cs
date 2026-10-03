using CINEMA_MANAGEMENT_SYSTEM.DTOs.MovieDTOs;

namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.GenreDTOs
{
    public class GenreWithMoviesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<MovieDTO> Movies { get; set; } = new List<MovieDTO>();
    }
}

