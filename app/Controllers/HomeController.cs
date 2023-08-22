using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using MongoDB.Driver;

namespace app.Controllers;

public class HomeController : Controller
{
    private IMongoCollection<Message> _messageCollection;

    public HomeController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("DataBook");
        _messageCollection = database.GetCollection<Message>("Messages");
    }

    public ActionResult Index()
    {
        ViewBag.User = HttpContext.Session?.GetString("UserName");
        var messages = _messageCollection.Find(Builders<Message>.Filter.Empty).ToList();
        return View(messages);
    }
}
