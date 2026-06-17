using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Slug { get; set; }

        [StringLength(255)]
        public string? Icon { get; set; }

        public string? Description { get; set; }

        // Navigation
        public ICollection<Tour>? Tours { get; set; }
    }
}
