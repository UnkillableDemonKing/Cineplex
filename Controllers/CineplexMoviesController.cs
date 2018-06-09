using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoogleLogin.Models;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GoogleLogin.Controllers
{
 
    

    public class CineplexMoviesController : Controller
    {
        private readonly MoviesContext _context;
        
        public CineplexMoviesController(MoviesContext context)
        {
            _context = context;    
        }

        private CineplexMovie getmoviebytitle(string title)
        {
            var movie = _context.CineplexMovie.Where(m => m.Movie.Title.Contains(title));
                return movie.FirstOrDefault();
        }

        //Get Movie Title from MovieId
        private Movie getMovieId(int ID)
        {
            var m = _context.Movie.Where(z => z.MovieId == ID);
            return m.FirstOrDefault();
        }

        private Cineplex getCineplex(int ID)
        {
            var m = _context.Cineplex.Where(z => z.CineplexId == ID);
            return m.FirstOrDefault();
        }
        //Convert from Datetime to string
        private string DatetimeToString(DateTime? session)
        {
            string datetime = session.ToString();
            return datetime;
        }

        private SessionMovie getMovieTime(int ID)
        {
            var m = _context.SessionMovie.Where(z => z.SessionId == ID);
            return m.FirstOrDefault();
        }

        // GET: CineplexMovies
        public async Task<IActionResult> Index(string searchString)
        {
            var movies = from m in _context.CineplexMovie.Include(c => c.Cineplex).Include(c => c.Movie).Include(c => c.Session)
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Movie.Title.Contains(searchString));
            }
            return View(await movies.ToListAsync());
        }


        //Search function for searching via cineplex location
        public async Task<IActionResult> Cineplex(string searchString)
        {
            var movies1 = from m in _context.CineplexMovie.Include(c => c.Cineplex).Include(c => c.Movie).Include(c => c.Session)
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies1 = movies1.Where(s => s.Cineplex.Location.Contains(searchString));
            }
            return View(await movies1.ToListAsync());

        }


        [HttpPost]
        public IActionResult Index(CineplexMovie cm)
        {
            HttpContext.Session.SetString("MovieTitle", getMovieId(cm.MovieId).Title);
            HttpContext.Session.SetString("CineplexLocation", getCineplex(cm.CineplexId).Location);
            string Movietime = DatetimeToString(getMovieTime(cm.SessionId).Movietime);
            HttpContext.Session.SetString("SessionId", Movietime);
            //When the User provides these, redirect to the Booking Page
            return RedirectToAction("Booking", "cineplexmovies");
        }

        public IActionResult Booking(Booking book)
        {
           
            ViewBag.SessionId = HttpContext.Session.GetString("SessionId");
            ViewBag.MovieTitle = HttpContext.Session.GetString("MovieTitle");
            ViewBag.CineplexLocation = HttpContext.Session.GetString("CineplexLocation");
            return View();
        }

        [HttpPost]
        public IActionResult Booking()
        {

            ViewBag.SessionId = HttpContext.Session.GetString("SessionId");
            ViewBag.MovieTitle = HttpContext.Session.GetString("MovieTitle");
            ViewBag.CineplexLocation = HttpContext.Session.GetString("CineplexLocation");
            //HttpContext.Session.SetString("Quantity", book.SeatBooking);
            //HttpContext.Session.SetInt32("Cost", book.Cost);
            //HttpContext.Session.SetObjectAsJson("Cart", myCart);
            return RedirectToAction("Payment");
        }


        public IActionResult Payment(Booking book)
        {
            ViewBag.SessionId = HttpContext.Session.GetString("SessionId");
            ViewBag.MovieTitle = HttpContext.Session.GetString("MovieTitle");
            ViewBag.CineplexLocation = HttpContext.Session.GetString("CineplexLocation");

            return View();
        }


        [HttpPost]
        public IActionResult Payment()
        {
            ViewBag.SessionId = HttpContext.Session.GetString("SessionId");
            ViewBag.MovieTitle = HttpContext.Session.GetString("MovieTitle");
            ViewBag.CineplexLocation = HttpContext.Session.GetString("CineplexLocation");
            return View();
        }

        // GET: CineplexMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cineplexMovie = await _context.CineplexMovie.SingleOrDefaultAsync(m => m.CineplexId == id);
            if (cineplexMovie == null)
            {
                return NotFound();
            }

            return View(cineplexMovie);
        }


        public IActionResult BookingSummary()
        {
            ViewBag.SessionId = HttpContext.Session.GetString("SessionId");
            ViewBag.MovieTitle = HttpContext.Session.GetString("MovieTitle");
            ViewBag.CineplexLocation = HttpContext.Session.GetString("CineplexLocation");
            return View();
        }

        // GET: CineplexMovies/Create
        public IActionResult Create()
        {
            ViewData["CineplexId"] = new SelectList(_context.Cineplex, "CineplexId", "Location");
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "LongDescription");
            ViewData["SessionId"] = new SelectList(_context.SessionMovie, "SessionId", "SessionId");
            return View();
        }

        // POST: CineplexMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CineplexId,MovieId,SessionId")] CineplexMovie cineplexMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cineplexMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CineplexId"] = new SelectList(_context.Cineplex, "CineplexId", "Location", cineplexMovie.CineplexId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "LongDescription", cineplexMovie.MovieId);
            ViewData["SessionId"] = new SelectList(_context.SessionMovie, "SessionId", "SessionId", cineplexMovie.SessionId);
            return View(cineplexMovie);
        }

        // GET: CineplexMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cineplexMovie = await _context.CineplexMovie.SingleOrDefaultAsync(m => m.CineplexId == id);
            if (cineplexMovie == null)
            {
                return NotFound();
            }
            ViewData["CineplexId"] = new SelectList(_context.Cineplex, "CineplexId", "Location", cineplexMovie.CineplexId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "LongDescription", cineplexMovie.MovieId);
            ViewData["SessionId"] = new SelectList(_context.SessionMovie, "SessionId", "SessionId", cineplexMovie.SessionId);
            return View(cineplexMovie);
        }

        private void getCart()
        {
            List<Booking> booking = new List<Booking>();

        }


        // POST: CineplexMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CineplexId,MovieId,SessionId")] CineplexMovie cineplexMovie)
        {
            if (id != cineplexMovie.CineplexId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cineplexMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CineplexMovieExists(cineplexMovie.CineplexId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CineplexId"] = new SelectList(_context.Cineplex, "CineplexId", "Location", cineplexMovie.CineplexId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "LongDescription", cineplexMovie.MovieId);
            ViewData["SessionId"] = new SelectList(_context.SessionMovie, "SessionId", "SessionId", cineplexMovie.SessionId);
            return View(cineplexMovie);
        }

        // GET: CineplexMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cineplexMovie = await _context.CineplexMovie.SingleOrDefaultAsync(m => m.CineplexId == id);
            if (cineplexMovie == null)
            {
                return NotFound();
            }

            return View(cineplexMovie);
        }

        // POST: CineplexMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cineplexMovie = await _context.CineplexMovie.SingleOrDefaultAsync(m => m.CineplexId == id);
            _context.CineplexMovie.Remove(cineplexMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CineplexMovieExists(int id)
        {
            return _context.CineplexMovie.Any(e => e.CineplexId == id);
        }
    }
}
