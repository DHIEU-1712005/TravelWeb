using Microsoft.EntityFrameworkCore;
using TravelWeb.Models;

namespace TravelWeb.Data
{
    public class TravelDbContext : DbContext
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu mẫu Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Biển đảo", Slug = "bien-dao", Icon = "🏖️" },
                new Category { CategoryId = 2, Name = "Núi rừng", Slug = "nui-rung", Icon = "⛰️" },
                new Category { CategoryId = 3, Name = "Văn hóa", Slug = "van-hoa", Icon = "🏛️" },
                new Category { CategoryId = 4, Name = "Ẩm thực", Slug = "am-thuc", Icon = "🍜" },
                new Category { CategoryId = 5, Name = "Mạo hiểm", Slug = "mao-hiem", Icon = "🎢" }
            );

            // Seed Destinations
            modelBuilder.Entity<Destination>().HasData(
                new Destination { DestinationId = 1, Name = "Hạ Long", Country = "Việt Nam", City = "Quảng Ninh", Region = "Miền Bắc", IsFeatured = true, ThumbnailUrl = "/images/halong.jpg", Description = "Vịnh Hạ Long - kỳ quan thiên nhiên thế giới" },
                new Destination { DestinationId = 2, Name = "Đà Nẵng", Country = "Việt Nam", City = "Đà Nẵng", Region = "Miền Trung", IsFeatured = true, ThumbnailUrl = "/images/danang.jpg", Description = "Thành phố đáng sống nhất Việt Nam" },
                new Destination { DestinationId = 3, Name = "Phú Quốc", Country = "Việt Nam", City = "Kiên Giang", Region = "Miền Nam", IsFeatured = true, ThumbnailUrl = "/images/phuquoc.jpg", Description = "Đảo ngọc Phú Quốc thiên đường nghỉ dưỡng" },
                new Destination { DestinationId = 4, Name = "Sapa", Country = "Việt Nam", City = "Lào Cai", Region = "Miền Bắc", IsFeatured = true, ThumbnailUrl = "/images/sapa.jpg", Description = "Sapa - thị trấn trong mây" },
                new Destination { DestinationId = 5, Name = "Hội An", Country = "Việt Nam", City = "Quảng Nam", Region = "Miền Trung", IsFeatured = true, ThumbnailUrl = "/images/hoian.jpg", Description = "Phố cổ Hội An di sản văn hóa thế giới" },
                new Destination { DestinationId = 6, Name = "Đà Lạt", Country = "Việt Nam", City = "Lâm Đồng", Region = "Miền Trung", IsFeatured = true, ThumbnailUrl = "/images/dalat.jpg", Description = "Thành phố ngàn hoa Đà Lạt" }
            );

            // Seed Tours
            modelBuilder.Entity<Tour>().HasData(
                new Tour { TourId = 1, TourCode = "HN-HL-3N2D", Name = "Hà Nội - Hạ Long 3 ngày 2 đêm", DestinationId = 1, CategoryId = 1, DurationDays = 3, DurationNights = 2, PriceAdult = 3500000, PriceChild = 2500000, DiscountPercent = 10, DepartureLocation = "Hà Nội", TransportType = "Xe khách", ThumbnailUrl = "/images/tour-halong.jpg", IsFeatured = true, Description = "Khám phá vẻ đẹp kỳ vĩ của Vịnh Hạ Long với hành trình 3 ngày 2 đêm" },
                new Tour { TourId = 2, TourCode = "HCM-DN-4N3D", Name = "Đà Nẵng - Hội An - Bà Nà 4N3D", DestinationId = 2, CategoryId = 3, DurationDays = 4, DurationNights = 3, PriceAdult = 5500000, PriceChild = 4000000, DiscountPercent = 15, DepartureLocation = "TP.HCM", TransportType = "Máy bay", ThumbnailUrl = "/images/tour-danang.jpg", IsFeatured = true, Description = "Khám phá Đà Nẵng - Hội An - Bà Nà Hills" },
                new Tour { TourId = 3, TourCode = "HCM-PQ-3N2D", Name = "Phú Quốc - Đảo ngọc 3N2D", DestinationId = 3, CategoryId = 1, DurationDays = 3, DurationNights = 2, PriceAdult = 4800000, PriceChild = 3500000, DiscountPercent = 20, DepartureLocation = "TP.HCM", TransportType = "Máy bay", ThumbnailUrl = "/images/tour-phuquoc.jpg", IsFeatured = true, Description = "Nghỉ dưỡng tại đảo ngọc Phú Quốc" },
                new Tour { TourId = 4, TourCode = "HN-SP-2N3D", Name = "Sapa - Fansipan 2N3D", DestinationId = 4, CategoryId = 2, DurationDays = 3, DurationNights = 2, PriceAdult = 2800000, PriceChild = 2000000, DiscountPercent = 5, DepartureLocation = "Hà Nội", TransportType = "Xe khách", ThumbnailUrl = "/images/tour-sapa.jpg", IsFeatured = true, Description = "Chinh phục đỉnh Fansipan và khám phá Sapa mộng mơ" },
                new Tour { TourId = 5, TourCode = "DN-HA-2N1D", Name = "Hội An - Phố cổ 2N1D", DestinationId = 5, CategoryId = 3, DurationDays = 2, DurationNights = 1, PriceAdult = 1800000, PriceChild = 1200000, DiscountPercent = 0, DepartureLocation = "Đà Nẵng", TransportType = "Xe khách", ThumbnailUrl = "/images/tour-hoian.jpg", Description = "Khám phá phố cổ Hội An về đêm" },
                new Tour { TourId = 6, TourCode = "HCM-DL-3N2D", Name = "Đà Lạt - Thành phố ngàn hoa 3N2D", DestinationId = 6, CategoryId = 4, DurationDays = 3, DurationNights = 2, PriceAdult = 3200000, PriceChild = 2300000, DiscountPercent = 10, DepartureLocation = "TP.HCM", TransportType = "Xe khách", ThumbnailUrl = "/images/tour-dalat.jpg", IsFeatured = true, Description = "Tận hưởng khí hậu mát mẻ và cảnh sắc thơ mộng của Đà Lạt" }
            );

            // Seed Blogs
            modelBuilder.Entity<Blog>().HasData(
                new Blog { BlogId = 1, Title = "10 điểm đến không thể bỏ qua tại Việt Nam 2026", Slug = "10-diem-den-viet-nam-2026", Category = "Cẩm nang", Summary = "Khám phá top 10 điểm đến hấp dẫn nhất Việt Nam", ThumbnailUrl = "/images/blog1.jpg", Content = "Việt Nam có rất nhiều điểm đến tuyệt vời..." },
                new Blog { BlogId = 2, Title = "Kinh nghiệm du lịch Phú Quốc tự túc", Slug = "kinh-nghiem-du-lich-phu-quoc", Category = "Kinh nghiệm", Summary = "Tổng hợp kinh nghiệm du lịch Phú Quốc từ A-Z", ThumbnailUrl = "/images/blog2.jpg", Content = "Phú Quốc đảo ngọc..." },
                new Blog { BlogId = 3, Title = "Ẩm thực Hội An - những món ăn đặc sản", Slug = "am-thuc-hoi-an", Category = "Ẩm thực", Summary = "Khám phá ẩm thực phố cổ Hội An", ThumbnailUrl = "/images/blog3.jpg", Content = "Cao lầu, mì Quảng, bánh mì Phượng..." }
            );
        }
    }
}
