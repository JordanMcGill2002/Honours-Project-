using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Services;
using Films.Models;
using MongoDB.Bson;
using System.Linq;
using System.Diagnostics;


namespace Films.Pages
{
    public class UpdateFilmModel : PageModel
    {
        private readonly FilmsService _filmsService;
        private readonly ActorsService _actorsService; // Add this back

        [BindProperty]
        public Film Film { get; set; }

        public Dictionary<ObjectId, Actor> Actors { get; set; } // Add this back

        public UpdateFilmModel(FilmsService filmsService, ActorsService actorsService) // Add ActorsService to the constructor
        {
            _filmsService = filmsService;
            _actorsService = actorsService; // Initialize _actorsService
        }

        public IActionResult OnGet(string id)
        {

           
            var actors = _actorsService.Get(); // Fetch the actors
            Actors = actors.ToDictionary(actor => actor._id); // Populate the Actors dictionary

            Console.WriteLine("OnPost method started");
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
            Console.WriteLine("OnPostUpdateNewFilm method started");
            Stopwatch.StartNew();

            Film existingFilm = _filmsService.Get(id);

            if (existingFilm != null)
            {
                existingFilm.Title = Film.Title;
                existingFilm.ReleaseDate = Film.ReleaseDate;
                existingFilm.Genre = Film.Genre;
                existingFilm.AgeRating = Film.AgeRating;
                existingFilm.Rating = Film.Rating;
                existingFilm.Actor_Id = Film.Actor_Id; // Update the Actor_Id

                _filmsService.Update(id, existingFilm);
                stopwatch.Stop();
            }
            Console.WriteLine($"Time Taken: {stopwatch.Elapsed.TotalMilliseconds} Milliseconds");
            Console.WriteLine($"Time Taken To Update : {stopwatch.Elapsed.TotalSeconds} Seconds");
            return RedirectToPage("./Index");
        }

    }
}
