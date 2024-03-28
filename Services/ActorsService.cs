using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Films.Models;

namespace Films.Services
{
    public class ActorsService
    {
        private readonly IMongoCollection<Actor> _actors;

        public ActorsService(IMongoDatabase database)
        {
            _actors = database.GetCollection<Actor>("Actors");
        }

        public List<Actor> Get() =>
            _actors.Find(actor => true).ToList();

        public Actor Get(ObjectId id) =>
            _actors.Find<Actor>(actor => actor._id == id).FirstOrDefault();

        public List<Actor> Get(List<ObjectId> ids) =>
            _actors.Find(actor => ids.Contains(actor._id)).ToList();

        // Add other necessary methods as per your requirements
    }
}
