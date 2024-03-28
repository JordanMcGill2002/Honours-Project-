using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using Films.Models;
using System.Threading.Tasks;
using Films.Pages;
using System;
using System.Linq;

namespace Films.Services
{
    public class FilmsService
    {
        private readonly IMongoCollection<Film> _films;

        public FilmsService(IMongoDatabase database)
        {
            _films = database.GetCollection<Film>("Films");
        }
     public List<Film> Get(FilterDefinition<Film> filter = null)
    {
        if (filter == null)
        {
            // If no filter is provided, return all films
            return _films.Find(film => true).ToList();
        }
        else
        {
            // If a filter is provided, return the matching films
            return _films.Find(filter).ToList();
        }
    }
        public List<Film> Get()
        {
            return _films.Find(film => true).ToList();
        }

        public Film Get(string id)
        {
            var objectId = new ObjectId(id);
            return _films.Find<Film>(film => film._id == objectId).FirstOrDefault();
        }

        public void Update(string id, Film filmIn)
        {
            var objectId = new ObjectId(id);
            _films.ReplaceOne(film => film._id == objectId, filmIn);
        }

      public void Remove(ObjectId id)
{
    _films.DeleteOne(film => film._id == id);
}
        public Film Create(Film film)
        {
            _films.InsertOne(film);
            return film;
        }

       public int GetMaxFilmNumber()
{
    // Fetch all film titles
    var filmTitles = _films.Find(film => true)
                           .ToList()
                           .Select(film => film.Title)
                           .ToList();

    // Extract the numbers from the titles
    var filmNumbers = filmTitles.Select(title =>
    {
        if (title.StartsWith("Film "))
        {
            if (int.TryParse(title.Substring(5), out int number))
            {
                return number;
            }
        }

        return 0;
    });

    // Find the max film number
    var maxFilmNumber = filmNumbers.Any() ? filmNumbers.Max() : 0;

    return maxFilmNumber;
}




    }
}
