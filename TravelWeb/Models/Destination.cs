using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models
{
    [Table("Destinations")]
    public class Destination
    {
        [Key]
        public int DestinationId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Slug { get; set; }

        [Required, StringLength(100)]
        public string Country { get; set; } = "Việt Nam";

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        public string? Description { get; set; }

        [StringLength(255)]
        public string? ThumbnailUrl { get; set; }

        public int ViewCount { get; set; } = 0;

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Tour>? Tours { get; set; }
    }
}
