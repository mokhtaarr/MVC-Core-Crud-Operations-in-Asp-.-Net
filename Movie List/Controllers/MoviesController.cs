using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_List.Dtos;
using Movie_List.Models;

namespace Movie_List.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplictionDbContext _context;
        public MoviesController(ApplictionDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Movies = await _context.Movies.ToListAsync();
            return View(Movies);
        }

        public async Task<IActionResult> Create()
        {
            var Movies = new MovieDto
            {
                Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync()

            };
            return View(Movies);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieDto model)
        {

            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                return View(model);

            }

            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("poster", "please Select Movie Poster");
                return View(model);

            }

            var poster = files.FirstOrDefault();
            var allowedExtentations = new List<string> { ".jpg", ".png" };

            if (!allowedExtentations.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("poster", "only jpg or png extention is allowed");
                return View(model);

            }

            return View(model);

        }
    }
    
}
