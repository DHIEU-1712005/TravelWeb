using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;

namespace TravelWeb.Controllers
{
    public class TourController : Controller
    {
        private readonly TravelDbContext _context;

        public TourController(TravelDbContext context)
        {
            _context = context;
        }

        // GET: /Tour - Danh sách tour
        public async Task<IActionResult> Index(string? search, int? categoryId, int? destinationId, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            int pageSize = 9;
            var query = _context.Tours
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.Status == "active");

            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Name.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId);

            if (destinationId.HasValue)
                query = query.Where(t => t.DestinationId == destinationId);

            if (minPrice.HasValue)
                query = query.Where(t => t.PriceAdult >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(t => t.PriceAdult <= maxPrice);

            int total = await query.CountAsync();
            var tours = await query
                .OrderByDescending(t => t.IsFeatured)
                .ThenByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.DestinationId = destinationId;

            return View(tours);
        }

        // GET: /Tour/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Include(t => t.Reviews!)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(t => t.TourId == id);

            if (tour == null) return NotFound();

            // Tăng lượt xem
            tour.ViewCount++;
            await _context.SaveChangesAsync();

            // Tour liên quan
            ViewBag.RelatedTours = await _context.Tours
                .Include(t => t.Destination)
                .Where(t => t.CategoryId == tour.CategoryId && t.TourId != tour.TourId)
                .Take(3)
                .ToListAsync();

            return View(tour);
        }
    }
}
