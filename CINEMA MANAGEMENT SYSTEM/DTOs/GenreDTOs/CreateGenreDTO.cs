using System.ComponentModel.DataAnnotations;

namespace CINEMA_MANAGEMENT_SYSTEM.DTOs.GenreDTOs
{
    public class CreateGenreDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}

