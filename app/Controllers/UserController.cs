using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

public class UserController : Controller
{
    private readonly IMongoCollection<User> _userCollection;

    public UserController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("DataBook");
        _userCollection = database.GetCollection<User>("Users");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if(user.Username.Trim().Length < 5)
        {
            ModelState.AddModelError("", "Username length must be minimum 5 symbols.");
            return View(user);
        }

        if (user.Password.Trim().Length < 5)
        {
            ModelState.AddModelError("", "Password length must be minimum 5 symbols.");
            return View(user);
        }

        var userExists = await _userCollection.Find(u => u.Username == user.Username).AnyAsync();

        if (!userExists)
        {
            var newUser = new User
            {
                Username = user.Username,
                Password = user.Password
            };

            await _userCollection.InsertOneAsync(newUser);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "User with this username is already exists.");
            return View(user);
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();

        if (user != null && password == user.Password)
        {
            HttpContext.Session.SetString("UserName", user.Username);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}


