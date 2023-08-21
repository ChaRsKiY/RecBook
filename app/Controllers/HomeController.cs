using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using MongoDB.Driver;

namespace app.Controllers;

public class HomeController : Controller
{
    private IMongoCollection<Movie> _movieCollection;

    public HomeController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MovieDatabase");
        _movieCollection = database.GetCollection<Movie>("Movies");
    }

    public ActionResult Index()
    {
        var movies = _movieCollection.Find(Builders<Movie>.Filter.Empty).ToList();
        return View(movies);
    }
}
