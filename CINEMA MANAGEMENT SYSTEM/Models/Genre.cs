using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CINEMA_MANAGEMENT_SYSTEM.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
