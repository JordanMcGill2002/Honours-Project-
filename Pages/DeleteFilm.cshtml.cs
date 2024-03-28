using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Services;
using Films.Models;
using System.Diagnostics;

namespace Films.Pages
{
    public class DeleteFilmModel : PageModel
    {
        private readonly FilmsService _filmsService;

        [BindProperty]
        public Film Film { get; set; }

        public DeleteFilmModel(FilmsService filmsService)
        {
            _filmsService = filmsService;
        }

        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Film = _filmsService.Get(id);

            if (Film == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(string id)
        {

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (id == null)
            {
                return NotFound();
            }

            Film = _filmsService.Get(id);

            if (Film != null)
            {
                 _filmsService.Remove(Film._id);
            }
             Console.WriteLine($"Time Taken: {stopwatch.Elapsed.TotalMilliseconds} Milliseconds");
            Console.WriteLine($"Time Taken To Delete 1 Record : {stopwatch.Elapsed.TotalSeconds} Seconds");
           
            return RedirectToPage("./Index");
        }
    }
}
