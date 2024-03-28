using System;
using System.Collections.Generic;
using System.IO; // Add this for File operations
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Services;
using Films.Models;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Films.Pages
{
    public class UpdateBulkModel : PageModel
    {
        private readonly FilmsService _filmsService;
        private readonly ActorsService _actorsService;
        public List<Film> Films { get; set; }
        public Dictionary<ObjectId, Actor> Actors { get; set; }

        [BindProperty]
        public Film InsMultiFilm { get; set; }
        public UpdateBulkModel(FilmsService filmsService, ActorsService actorsService)
        {
            _filmsService = filmsService;
            _actorsService = actorsService;
        }
       public IActionResult OnGet()
        {
            Films = _filmsService.Get();
            var actors = _actorsService.Get();
            Actors = actors.ToDictionary(actor => actor._id);
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPost(int recordCount)
        {
            List<double> timesTaken = new List<double>();
            StringBuilder sb = new StringBuilder(); // Declare sb here

            for (int j = 0; j < 10; j++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var films = _filmsService.Get();
                var random = new Random();
                var genres = new[] { "Horror", "Action", "Sci-Fi", "Comedy", "Romance", "Musical", "Thriller" };
                var ageRatings = new[] { "U", "PG", "12-A", "15", "18" };
                double randomRating = Math.Round(1 + random.NextDouble() * 9, 1);

                for (int i = 0; i < recordCount; i++)
                {
                    if (films.Count == 0)
                    {
                        Console.WriteLine("No more films to update.");
                        break;
                    }

                    int indexToUpdate = random.Next(films.Count);
                    Film filmToUpdate = films[indexToUpdate];

                    // Randomly select a field to update
                    int fieldToUpdate = random.Next(5);
                    switch (fieldToUpdate)
                    {
                        case 0:
                            filmToUpdate.Title = $"Updated Film {random.Next(10000)}";
                            break;
                        case 1:
                            filmToUpdate.Rating = randomRating;
                            break;
                        case 2:
                            filmToUpdate.AgeRating = ageRatings[random.Next(ageRatings.Length)];
                            break;
                        case 3:
                            filmToUpdate.ReleaseDate = DateTime.Now.AddDays(-random.Next(365 * 50)); // Random date in the last 50 years
                            break;
                    }

                    _filmsService.Update(filmToUpdate._id.ToString(), filmToUpdate); // Convert ObjectId to string

                    Console.WriteLine($"Updated film: {filmToUpdate.Title}");
                }

                stopwatch.Stop();
                
                TempData["MessageUpdate"] = $"Record successfully Updated.\\n\\n\\n Time taken to update records: {stopwatch.Elapsed.TotalSeconds} seconds";

                sb.AppendLine($"Update {j+1}: {stopwatch.Elapsed.TotalSeconds} Seconds");
            }

            string fileName = $"Update{recordCount}.txt";
            System.IO.File.WriteAllText(fileName, sb.ToString()); // Now sb is accessible here

            return RedirectToPage("index");
        }
    }
}
