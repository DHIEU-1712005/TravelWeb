using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TravelDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(TravelDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Upload ảnh tour - trả về đường dẫn lưu
        private async Task<string?> SaveImage(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var ext = Path.GetExtension(file.FileName).ToLower();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowed.Contains(ext)) return null;

            var fileName = $"tour_{DateTime.Now.Ticks}{ext}";
            var folder = Path.Combine(_env.WebRootPath, "images", "tours");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/tours/{fileName}";
        }

        // ========== DASHBOARD ==========
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalTours = await _context.Tours.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.TotalRevenue = await _context.Bookings
                .Where(b => b.PaymentStatus == "paid")
                .SumAsync(b => (decimal?)b.FinalAmount) ?? 0;

            ViewBag.PendingBookings = await _context.Bookings.CountAsync(b => b.BookingStatus == "pending");
            ViewBag.TotalContacts = await _context.Contacts.CountAsync(c => c.Status == "new");

            ViewBag.RecentBookings = await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.User)
                .OrderByDescending(b => b.BookedAt)
                .Take(5)
                .ToListAsync();

            ViewBag.TopTours = await _context.Tours
                .OrderByDescending(t => t.BookingCount)
                .Take(5)
                .ToListAsync();

            return View();
        }

        // ========== QUẢN LÝ TOUR ==========
        public async Task<IActionResult> Tours(string? search)
        {
            var query = _context.Tours
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Name.Contains(search));

            var tours = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
            ViewBag.Search = search;
            return View(tours);
        }

        public async Task<IActionResult> CreateTour()
        {
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(new Tour());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTour(Tour tour, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var imagePath = await SaveImage(imageFile);
                if (imagePath != null) tour.ThumbnailUrl = imagePath;

                tour.CreatedAt = DateTime.Now;
                tour.Status = "active";
                _context.Tours.Add(tour);
                await _context.SaveChangesAsync();
                TempData["Success"] = "✅ Thêm tour mới thành công!";
                return RedirectToAction(nameof(Tours));
            }
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(tour);
        }

        public async Task<IActionResult> EditTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null) return NotFound();
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTour(Tour tour, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Upload ảnh mới nếu có chọn file
                var imagePath = await SaveImage(imageFile);
                if (imagePath != null)
                {
                    tour.ThumbnailUrl = imagePath;
                }
                // Nếu KHÔNG chọn file mới → giữ nguyên ảnh cũ (tour.ThumbnailUrl từ hidden input)

                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();
                TempData["Success"] = "✅ Cập nhật tour thành công!";
                return RedirectToAction(nameof(Tours));
            }
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
                TempData["Success"] = "🗑️ Đã xóa tour!";
            }
            return RedirectToAction(nameof(Tours));
        }

        // ========== QUẢN LÝ BOOKING ==========
        public async Task<IActionResult> Bookings(string? status)
        {
            var query = _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(b => b.BookingStatus == status);

            var bookings = await query.OrderByDescending(b => b.BookedAt).ToListAsync();
            ViewBag.Status = status;
            return View(bookings);
        }

        public async Task<IActionResult> BookingDetail(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Tour)
                    .ThenInclude(t => t!.Destination)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.BookingStatus = status;
                if (status == "confirmed")
                {
                    booking.PaymentStatus = "paid";
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = $"✅ Đã cập nhật trạng thái: {status}";
            }
            return RedirectToAction(nameof(Bookings));
        }

        // ========== QUẢN LÝ USER ==========
        public async Task<IActionResult> Users(string? search)
        {
            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(u => u.FullName.Contains(search) || u.Email.Contains(search));

            var users = await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
            ViewBag.Search = search;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Status = user.Status == "active" ? "banned" : "active";
                await _context.SaveChangesAsync();
                TempData["Success"] = $"✅ Đã {(user.Status == "active" ? "mở khóa" : "khóa")} tài khoản!";
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.RoleId != 1)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "🗑️ Đã xóa user!";
            }
            return RedirectToAction(nameof(Users));
        }
    }
}