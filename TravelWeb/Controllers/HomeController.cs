using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly TravelDbContext _context;

        public HomeController(TravelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.FeaturedTours = await _context.Tours
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Where(t => t.IsFeatured && t.Status == "active")
                .Take(6)
                .ToListAsync();

            ViewBag.FeaturedDestinations = await _context.Destinations
                .Where(d => d.IsFeatured)
                .Take(6)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();

            ViewBag.LatestBlogs = await _context.Blogs
                .Where(b => b.Status == "published")
                .OrderByDescending(b => b.CreatedAt)
                .Take(3)
                .ToListAsync();

            return View();
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cảm ơn bạn! Chúng tôi sẽ liên hệ lại sớm nhất.";
                return RedirectToAction(nameof(Contact));
            }
            return View(contact);
        }

        public IActionResult Error() => View();
    }
}
