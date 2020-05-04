using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackOverflowClone.Data;

namespace StackOverflowClone.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;
        public AccountController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr");
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            var repos = new AccountRepository(_connectionString);
            repos.AddUser(user, password);
            return Redirect("/");
        }
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(User user, string password)
        {
            var repos = new AccountRepository(_connectionString);
            var result = repos.LogIn(user.Email, password);
            if(result == null)
            {
                TempData["Error"] = "Invalid LogIn";
            }
            var claims = new List<Claim>
            {
                new Claim("user", result.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/newQuestion");
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}