using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

public class EditController : Controller
{
    private readonly IMongoCollection<Movie> _movieCollection;

    public EditController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MovieDatabase");
        _movieCollection = database.GetCollection<Movie>("Movies");
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        var movie = _movieCollection.Find(m => m.Id == id).FirstOrDefault();
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    [HttpPost]
    public IActionResult Edit(string id, string imagename, Movie updatedMovie, IFormFile imageFile)
    {
        if (id != updatedMovie.Id)
        {
            return BadRequest();
        }

        if (imageFile != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine("wwwroot/movies", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            updatedMovie.ImageName = fileName;
        }
        else
        {
            updatedMovie.ImageName = imagename;
        }

        _movieCollection.ReplaceOne(m => m.Id == id, updatedMovie);

        return RedirectToAction("Index", "Home");
    }
}


