using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [StringLength(20)]
        public string? BookingCode { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int TourId { get; set; }
        [ForeignKey("TourId")]
        public Tour? Tour { get; set; }

        [Required, Display(Name = "Số người lớn")]
        [Range(1, 50)]
        public int NumAdults { get; set; } = 1;

        [Display(Name = "Số trẻ em")]
        [Range(0, 50)]
        public int NumChildren { get; set; } = 0;

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal FinalAmount { get; set; }

        [Required, StringLength(100), Display(Name = "Họ tên người liên hệ")]
        public string ContactName { get; set; } = string.Empty;

        [Required, StringLength(20), Display(Name = "Số điện thoại")]
        public string ContactPhone { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100), Display(Name = "Email")]
        public string ContactEmail { get; set; } = string.Empty;

        [Display(Name = "Yêu cầu đặc biệt")]
        public string? SpecialRequests { get; set; }

        public DateTime DepartureDate { get; set; }

        [StringLength(20)]
        public string BookingStatus { get; set; } = "pending";

        [StringLength(20)]
        public string PaymentStatus { get; set; } = "unpaid";

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        public DateTime BookedAt { get; set; } = DateTime.Now;
    }
}
