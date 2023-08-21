using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Movie
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Title { get; set; }
    public string Producer { get; set; }
    public string Genre { get; set; }
    public int Year { get; set; }
    public string Stars { get; set; }
    public string Description { get; set; }
    public string ImageName { get; set; }
}