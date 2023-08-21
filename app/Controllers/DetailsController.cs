using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using MongoDB.Driver;

namespace app.Controllers;

public class DetailsController : Controller
{
    private IMongoCollection<Movie> _movieCollection;

    public DetailsController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MovieDatabase");
        _movieCollection = database.GetCollection<Movie>("Movies");
    }


    [HttpGet]
    public IActionResult Details(string id)
    {
        var movie = _movieCollection.Find(m => m.Id == id).FirstOrDefault();
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }
}
