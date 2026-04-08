using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News_Site.Models.Entity;
using System.Security.Cryptography.Xml;
using System.Diagnostics;
using Microsoft.Identity.Client;
using News_Site.Models.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace News_Site.Controllers
{
    public class UserController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();

        //Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string password)
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == UserName);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Giriş yapan admin mi, kontrol et:
                if (user.IsAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewData["ErrorMessage"] = "Hatalı kullanıcı adı veya şifre";
            return View();
        }


        //Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.UserName = model.UserName;
                user.UserSurname = model.UserSurname;
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                user.UserEmail = model.UserEmail;
                db.Users.Add(user);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Registiration successfully!";
                return RedirectToAction("Login");
            }
            return RedirectToAction("Register");
        }

    }
}
