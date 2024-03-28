using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System;

namespace Films.Models
{
 public class Actor
{
    public ObjectId _id { get; set; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    // Other properties...
}

public class Film
{
    public ObjectId _id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string AgeRating { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double Rating { get; set; }
     public ObjectId Actor_Id { get; set; }
    public Actor Actor { get; set; }
    // Other properties...
}}
