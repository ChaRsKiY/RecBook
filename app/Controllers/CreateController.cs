using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using MongoDB.Driver;

namespace app.Controllers;

public class CreateController : Controller
{
    private IMongoCollection<Movie> _movieCollection;

    public CreateController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MovieDatabase");
        _movieCollection = database.GetCollection<Movie>("Movies");
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Movie movie, IFormFile imageFile)
    {
        if (imageFile != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine("wwwroot/movies", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            movie.ImageName = fileName;
        }

        _movieCollection.InsertOne(movie);
        return RedirectToAction("Index", "Home");
    }
}
