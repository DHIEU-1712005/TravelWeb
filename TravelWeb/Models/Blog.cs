using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models
{
    [Table("Blogs")]
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required, StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Slug { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        public string? Summary { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Content { get; set; }

        [StringLength(255)]
        public string? ThumbnailUrl { get; set; }

        public int ViewCount { get; set; } = 0;

        [StringLength(20)]
        public string Status { get; set; } = "published";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
