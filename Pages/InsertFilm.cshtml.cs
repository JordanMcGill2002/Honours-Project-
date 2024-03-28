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
    public class InsertFilm : PageModel
    {
        private readonly FilmsService _filmsService;
        private readonly ActorsService _actorsService;
        public List<Film> Films { get; set; }
        public Dictionary<ObjectId, Actor> Actors { get; set; } // Changed from List<Actor> to Dictionary<ObjectId, Actor>

        [BindProperty]
        public Film InsFilm { get; set; }

        public InsertFilm(FilmsService filmsService, ActorsService actorsService)
        {
            _filmsService = filmsService;
            _actorsService = actorsService;
        }

        public void OnGet()
        {
            Films = _filmsService.Get();
            var actors = _actorsService.Get();
            Actors = actors.ToDictionary(actor => actor._id);
            
        }

        public IActionResult OnPost()
        {

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("OnPostInsertNewFilm method started");

            DateTime releaseDate = DateTime.Parse(Request.Form["ReleaseDate"].ToString()).Date;
            double rating = double.Parse(Request.Form["Rating"].ToString());

            string actorIdString = Request.Form["Actor"].ToString();
            if (actorIdString != null && ObjectId.TryParse(actorIdString, out ObjectId actorId))
            {
                // Parsing was successful, actorId now contains the parsed ObjectId
                Console.WriteLine($"Parsed actor ID: {actorId}");

                // Create a new Film object from the form data
                InsFilm = new Film
                {
                    Title = Request.Form["tbxTitle"],
                    ReleaseDate = releaseDate,
                    AgeRating = Request.Form["AgeRating"],
                    Genre = Request.Form["Genre"],
                    Rating = rating,
                    Actor_Id = actorId
                };
                Film newFilm = _filmsService.Create(InsFilm); // Create a new film record

                // Check if Actors and InsFilm.Actor_Id are not null and if the actor exists in the dictionary
               
                    // Log the state of NewFilm before inserting
                    Console.WriteLine($"Inserting new film: {InsFilm.Title}");

                 
                 stopwatch.Stop();
                }
                
                    // Handle the case where Actors or InsFilm.Actor_Id is null or the actor does not exist
                Console.WriteLine($"Time Taken: {stopwatch.Elapsed.TotalMilliseconds} Milliseconds");
            Console.WriteLine($"Time Taken To Insert 1 Record : {stopwatch.Elapsed.TotalSeconds} Seconds");
           
                    return RedirectToPage("index"); // Return to the page with an error message

        }
    }
}
