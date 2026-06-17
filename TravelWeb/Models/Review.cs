using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int TourId { get; set; }
        [ForeignKey("TourId")]
        public Tour? Tour { get; set; }

        [StringLength(200)]
        public string? Title { get; set; }

        public string? Content { get; set; }

        [Range(1, 5)]
        public byte Rating { get; set; } = 5;

        [StringLength(20)]
        public string Status { get; set; } = "approved";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
