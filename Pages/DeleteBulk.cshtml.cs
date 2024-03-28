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
    public class DeleteBulkModel : PageModel
    {
        private readonly FilmsService _filmsService;
        private readonly ActorsService _actorsService;
        public List<Film> Films { get; set; }
        public Dictionary<ObjectId, Actor> Actors { get; set; }

        [BindProperty]
        public Film InsMultiFilm { get; set; }
        public DeleteBulkModel(FilmsService filmsService, ActorsService actorsService)
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

        for (int i = 0; i < recordCount; i++)
        {
            if (films.Count == 0)
            {
                Console.WriteLine("No more films to delete.");
                break;
            }

            int indexToDelete = random.Next(films.Count);
            Film filmToDelete = films[indexToDelete];

            _filmsService.Remove(filmToDelete._id);

            Console.WriteLine($"Deleting film: {filmToDelete.Title}");

            // Remove the film from the local list to keep it in sync with the database
            films.RemoveAt(indexToDelete);
        }

        stopwatch.Stop();
        double timeTaken = stopwatch.Elapsed.TotalSeconds;
        timesTaken.Add(timeTaken); // Store the time taken for this iteration

        
        TempData["MessageDelete"] = $"Record successfully Deleted.\\n\\n\\n Time taken to Delete  records: {timeTaken} seconds ";

        sb.AppendLine($"Time Taken To Delete {recordCount} : {timeTaken} Seconds");
    }

    string fileName = $"Delete_{recordCount}.txt";
    System.IO.File.WriteAllText(fileName, sb.ToString()); // Now sb is accessible here

    return RedirectToPage("index");
}
    }}