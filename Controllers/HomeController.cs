using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private E_Context dbContext;

        public HomeController(E_Context context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("registering")]
        public IActionResult Registering(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return Register();
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return Register();
            }
        }


        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost("logging")]
        public IActionResult Logging(LogUser user)
        {
            if(ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return Login();
                }
                PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
                PasswordVerificationResult Result = Hasher.VerifyHashedPassword(user, userInDb.Password, user.Password);
                if(Result == 0)
                {
                    ModelState.AddModelError("LogUser", "Invalid Email/Password");
                    return Login();
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return Login();
            }
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? LoggedUser = HttpContext.Session.GetInt32("UserId");
            if(LoggedUser == null)
            {
                return RedirectToAction("Register");
            }
            User userInSession = dbContext.Users.FirstOrDefault(u => u.UserId == (int)LoggedUser);
        
            return View("Dashboard", userInSession);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            int? LoggedUser = HttpContext.Session.GetInt32("UserId");
            if(LoggedUser == null)
            {
                return RedirectToAction("Register");
            }
            HttpContext.Session.Clear();
            return RedirectToAction("LoggedOut");
        }

        [HttpGet("loggedout")]
        public IActionResult LoggedOut()
        {
            return View("LoggedOut");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
