using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Services;
using Films.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Films.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FilmsService _filmsService;
        private readonly ActorsService _actorsService;

        public List<Film> Films { get; set; }
        public Dictionary<ObjectId, Actor> Actors { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public IndexModel(FilmsService filmsService, ActorsService actorsService)
        {
            _filmsService = filmsService;
            _actorsService = actorsService;
        }
        
        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                // Check if SearchCriteria is null or empty, and set a default field if it is
                if (string.IsNullOrEmpty(SearchCriteria))
                {
                    SearchCriteria = "Title"; // or any other default field
                }

                // Perform the search
                FilterDefinition<Film> filter;
                if (SearchCriteria == "Rating")
                {
                    if (double.TryParse(SearchQuery, out double ratingValue))
                    {
                        filter = Builders<Film>.Filter.Eq(SearchCriteria, ratingValue);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    filter = Builders<Film>.Filter.Regex(SearchCriteria, new BsonRegularExpression(SearchQuery, "i"));
                }
                Films = _filmsService.Get(filter);
            }
            else
            {
                Films = _filmsService.Get();
            }

            var actorIds = Films.Select(film => film.Actor_Id).Distinct().ToList();
            Actors = _actorsService.Get(actorIds).ToDictionary(actor => actor._id);
        }
    }
}
