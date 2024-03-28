using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Services;
using Films.Models;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks; // Corrected here
using System.Diagnostics;
using System.IO;
using System.Text;


namespace Films.Pages
{
    public class InsertBulkModel : PageModel
    {
        private readonly FilmsService _filmsService;
        private readonly ActorsService _actorsService;
        public List<Film> Films { get; set; }
        public Dictionary<ObjectId, Actor> Actors { get; set; } // Changed from List<Actor> to Dictionary<ObjectId, Actor>


        [BindProperty]
        public Film InsMultiFilm { get; set; }
        public InsertBulkModel(FilmsService filmsService, ActorsService actorsService)
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

        public IActionResult OnPost(int recordCount)
        {


            var films = new List<Film>();
            var random = new Random();
            var genres = new[] { "Horror", "Action", "Sci-Fi", "Comedy", "Romance", "Musical", "Thriller" };
            var ageRatings = new[] { "U", "PG", "12-A", "15", "18" };
            Random rand = new Random();
            int range = (DateTime.Today - new DateTime(1900, 1, 1)).Days;

            // Define the ObjectId of the actors
            var actorIds = new List<ObjectId>
         {
            new ObjectId("65d5cf9dc39cd69e4c31d704"),
            new ObjectId("65e8aa61d45cde67eaf42e3b"),
            new ObjectId("65e8aa61d45cde67eaf42e3c"),
            new ObjectId("65e8aa61d45cde67eaf42e3d"),
            new ObjectId("65e8aa61d45cde67eaf42e3e"),
            new ObjectId("65e8aa61d45cde67eaf42e3f"),
            new ObjectId("65e8aa61d45cde67eaf42e40"),
            new ObjectId("65e8aa61d45cde67eaf42e41"),
            new ObjectId("65e8aa61d45cde67eaf42e42"),
            new ObjectId("65e8aa61d45cde67eaf42e43"),
            new ObjectId("65e8aa61d45cde67eaf42e44")
         };

            int maxFilmNumber = _filmsService.GetMaxFilmNumber();
            // Get the current process
var currentProcess = Process.GetCurrentProcess();

List<TimeSpan> cpuTimes = new List<TimeSpan>();
      var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
List<float> ramUsages = new List<float>();
List<double> timesTaken = new List<double>();

for (int j = 0; j < 10; j++)
{
    double totalSeconds = 0;
    for (int i = 0; i < recordCount; i++)
    {
        DateTime randomDate = new DateTime(1900, 1, 1).AddDays(rand.Next(range));

        // Get the initial CPU time
        var initialCpuTime = currentProcess.TotalProcessorTime;

        float initialRamUsage = ramCounter.NextValue();

        var film = new Film
        {
            Title = $"Film {maxFilmNumber + 1}",
            Genre = genres[random.Next(genres.Length)],
            ReleaseDate = randomDate,
            AgeRating = ageRatings[random.Next(ageRatings.Length)],
            Rating = double.Parse(random.Next(0, 11).ToString()),
            Actor_Id = actorIds[random.Next(actorIds.Count)] // Select a random actor from the list
        };

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Film newFilm = _filmsService.Create(film);

        stopwatch.Stop();
        totalSeconds += stopwatch.Elapsed.TotalSeconds;

        // Get the final CPU time
        var finalCpuTime = currentProcess.TotalProcessorTime;

        // Calculate the CPU time difference
        var cpuTimeDifference = finalCpuTime - initialCpuTime;

        float finalRamUsage = ramCounter.NextValue();
        float ramUsageDifference = initialRamUsage - finalRamUsage; // RAM usage is expected to decrease

        cpuTimes.Add(cpuTimeDifference);
        ramUsages.Add(ramUsageDifference);
    }

    timesTaken.Add(totalSeconds);
    TempData["MessageInsert"] = $"Record successfully Inserted.\\n\\n\\n Time taken to Insert {recordCount} records: {totalSeconds} seconds \\n\\n\\n Number of Records Before Insert:  \\n\\n\\n Number of Records after Insert: ";
}

    StringBuilder sb = new StringBuilder();
sb.AppendLine("Times taken for each iteration:");

for (int i = 0; i < timesTaken.Count; i++)
{
    sb.AppendLine($"Iteration {i + 1}: {timesTaken[i]} seconds");
    sb.AppendLine($"CPU Time for record {i + 1}: {cpuTimes[i].TotalMilliseconds} ms");
    sb.AppendLine($"RAM Usage for record {i + 1}: {ramUsages[i]} MB");
}

// Write the times taken and performance usage to a text file
string fileName = $"Insert_Test_{recordCount}.txt";
System.IO.File.WriteAllText(fileName, sb.ToString());

return RedirectToPage("index");
        }
    }
}