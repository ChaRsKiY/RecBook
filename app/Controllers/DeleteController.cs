using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using MongoDB.Driver;

namespace app.Controllers;

public class DeleteController : Controller
{
    private IMongoCollection<Movie> _movieCollection;

    public DeleteController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MovieDatabase");
        _movieCollection = database.GetCollection<Movie>("Movies");
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        var movie = _movieCollection.Find(m => m.Id == id).FirstOrDefault();
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(string id)
    {
        _movieCollection.DeleteOne(m => m.Id == id);
        return RedirectToAction("Index", "Home");
    }
}
