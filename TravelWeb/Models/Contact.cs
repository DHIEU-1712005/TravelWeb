using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models
{
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required, StringLength(100), Display(Name = "Họ tên")]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20), Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [StringLength(200), Display(Name = "Chủ đề")]
        public string? Subject { get; set; }

        [Required, Display(Name = "Nội dung")]
        public string Message { get; set; } = string.Empty;

        [StringLength(20)]
        public string Status { get; set; } = "new";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
