using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models
{
    [Table("Tours")]
    public class Tour
    {
        [Key]
        public int TourId { get; set; }

        [StringLength(50)]
        public string? TourCode { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Slug { get; set; }

        public int DestinationId { get; set; }
        [ForeignKey("DestinationId")]
        public Destination? Destination { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Detail { get; set; }

        public int DurationDays { get; set; }
        public int DurationNights { get; set; }

        [StringLength(150)]
        public string? DepartureLocation { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal PriceAdult { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal? PriceChild { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercent { get; set; } = 0;

        public int MaxParticipants { get; set; } = 30;

        [StringLength(50)]
        public string? TransportType { get; set; }

        public string? Includes { get; set; }
        public string? Excludes { get; set; }
        public string? Notes { get; set; }

        [StringLength(255)]
        public string? ThumbnailUrl { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "active";

        [Column(TypeName = "decimal(3,2)")]
        public decimal RatingAvg { get; set; } = 0;

        public int ViewCount { get; set; } = 0;
        public int BookingCount { get; set; } = 0;
        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        // Computed property
        [NotMapped]
        public decimal FinalPrice => PriceAdult * (1 - DiscountPercent / 100);
    }
}
