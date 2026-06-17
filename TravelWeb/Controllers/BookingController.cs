using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TravelWeb.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly TravelDbContext _context;
        private readonly IConfiguration _config;

        public BookingController(TravelDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: /Booking/Create/5
        public async Task<IActionResult> Create(int tourId)
        {
            var tour = await _context.Tours
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.TourId == tourId);

            if (tour == null) return NotFound();

            ViewBag.Tour = tour;
            return View(new Booking { TourId = tourId, DepartureDate = DateTime.Now.AddDays(7) });
        }

        // POST: /Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var tour = await _context.Tours.FindAsync(booking.TourId);
            if (tour == null) return NotFound();

            if (ModelState.IsValid)
            {
                decimal adultPrice = tour.PriceAdult * (1 - tour.DiscountPercent / 100);
                decimal childPrice = (tour.PriceChild ?? 0) * (1 - tour.DiscountPercent / 100);

                booking.TotalAmount = booking.NumAdults * tour.PriceAdult + booking.NumChildren * (tour.PriceChild ?? 0);
                booking.FinalAmount = booking.NumAdults * adultPrice + booking.NumChildren * childPrice;
                booking.BookingCode = "BK" + DateTime.Now.ToString("yyyyMMddHHmmss");
                booking.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                booking.BookedAt = DateTime.Now;

                _context.Bookings.Add(booking);
                tour.BookingCount++;
                await _context.SaveChangesAsync();

                // Nếu chọn chuyển khoản → đi đến trang thanh toán
                if (booking.PaymentMethod == "bank_transfer")
                {
                    return RedirectToAction(nameof(Payment), new { id = booking.BookingId });
                }

                return RedirectToAction(nameof(Confirm), new { id = booking.BookingId });
            }

            ViewBag.Tour = tour;
            return View(booking);
        }

        // GET: /Booking/Payment/5 - Trang thanh toán chuyển khoản
        public async Task<IActionResult> Payment(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            // Lấy thông tin ngân hàng từ config
            var bankId = _config["BankInfo:BankId"];
            var bankName = _config["BankInfo:BankName"];
            var accountNo = _config["BankInfo:AccountNumber"];
            var accountName = _config["BankInfo:AccountName"];

            // Nội dung chuyển khoản
            var content = booking.BookingCode;

            // Tạo URL VietQR
            ViewBag.QrUrl = $"https://img.vietqr.io/image/{bankId}-{accountNo}-compact2.png" +
                            $"?amount={booking.FinalAmount}" +
                            $"&addInfo={content}" +
                            $"&accountName={Uri.EscapeDataString(accountName ?? "")}";

            ViewBag.BankName = bankName;
            ViewBag.AccountNumber = accountNo;
            ViewBag.AccountName = accountName;
            ViewBag.TransferContent = content;

            return View(booking);
        }

        // GET: /Booking/Confirm/5
        public async Task<IActionResult> Confirm(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Tour)
                    .ThenInclude(t => t!.Destination)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();
            return View(booking);
        }

        // GET: /Booking/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var bookings = await _context.Bookings
                .Include(b => b.Tour)
                    .ThenInclude(t => t!.Destination)
                .Where(b => b.UserId == userId)   // ← Lọc theo user
                .OrderByDescending(b => b.BookedAt)
                .ToListAsync();

            return View(bookings);
        }
    }
}