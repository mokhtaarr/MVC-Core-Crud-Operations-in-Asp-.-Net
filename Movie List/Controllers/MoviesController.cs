using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Movie_List.Dtos;
using Movie_List.Models;
using NToastNotify;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Movie_List.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplictionDbContext _context;
        private readonly IToastNotification _toastNotification;
        public MoviesController(ApplictionDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var Movies = await _context.Movies.OrderByDescending(m=>m.Rate).ToListAsync();
            return View(Movies);
        }

        public async Task<IActionResult> Create()
        {
            var Movies = new MovieDto
            {
                Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync()

            };
            return View("MovieForm", Movies);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieDto model)
        {

            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                return View("MovieForm",model);

            }

            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("poster", "please Select Movie Poster");
                return View("MovieForm", model);

            }

            var poster = files.FirstOrDefault();
            var allowedExtentations = new List<string> { ".jpg", ".png" };

            if (!allowedExtentations.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("poster", "only jpg or png extention is allowed");
                return View("MovieForm", model);

            }

            if (poster.Length>1048576)
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("poster", "poster can not be more than 1MB !");
                return View("MovieForm", model);

            }

            using var dataStream = new MemoryStream();
            await poster.CopyToAsync(dataStream);

            var movie = new Movie
            {
                Title = model.Title,
                GenreId = model.GenreId,
                year = model.year,
                Rate = model.Rate,
                StoreLine = model.StoreLine,
                poster = dataStream.ToArray()

            };

            _context.Movies.Add(movie);
            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Movie Created Successfully");


            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult>Edit(int? id)
        {
            if(id==null)
            {
                return BadRequest($"We don`t Found your id : {id}");
            }
            var movie = await _context.Movies.FindAsync(id);

            if(movie == null)
            {
                return BadRequest();
            }
            var Movies = new MovieDto
            {
                id = movie.Id,
                GenreId=movie.GenreId,
                Rate=movie.Rate,
                Title=movie.Title,
                year=movie.year,
                StoreLine=movie.StoreLine,
                poster=movie.poster,
                Genres=await _context.Genres.OrderBy(g=>g.Name).ToListAsync()

            };
            return View("MovieForm",Movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieDto model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
                return View("MovieForm", model);

            }
            var movie = await _context.Movies.FindAsync(model.id);

            if (movie == null)
                return BadRequest();
            
            movie.Title=model.Title;
            movie.StoreLine = model.StoreLine;
            movie.Rate = model.Rate;
            movie.GenreId = model.GenreId;
            movie.year = model.year;

            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Movie Updated Successfully");

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return BadRequest();

            var movie = await _context.Movies.Include(m=>m.genre).SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();
            return View(movie);

            

        }

        public async Task<IActionResult> delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return Ok();



        }



    }

}
