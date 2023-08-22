using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

public class CreateController : Controller
{
    private readonly IMongoCollection<Message> _messageCollection;

    public CreateController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("DataBook");
        _messageCollection = database.GetCollection<Message>("Messages");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Message message)
    {
        if (message.Text.Trim().Length < 5)
        {
            ModelState.AddModelError("", "Messsage length must be minimum 5 symbols.");
            return View(message);
        }

        string username = HttpContext.Session.GetString("UserName");

        if (username != null)
        {
            var newMessage = new Message
            {
                UserName = username,
                Text = message.Text,
                Time = DateTime.Now
            };

            await _messageCollection.InsertOneAsync(newMessage);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
}


