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
    }
    
}
