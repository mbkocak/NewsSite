using Microsoft.AspNetCore.Mvc;
using News_Site.Models.Entity;
using System.Diagnostics;
using Microsoft.AspNetCore.Antiforgery;
using System.Linq;
using News_Site.Models.ViewModel;


namespace News_Site.Controllers
{
    public class MembersController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();
        private readonly IAntiforgery _antiforgery;

        public MembersController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }


        public IActionResult Index(string UserName, string UserSurname, string Password, string UserEmail, bool? IsAdmin)
        {
            string username;
            string usersurname;
            string password;
            string useremail;
            bool isAdmin;
            IQueryable<User> users = db.Users;

            if (!string.IsNullOrEmpty(UserName))
            {
                string userLower = UserName.ToLower();
                users = users.Where(x => x.UserName.ToLower().Contains(userLower));
            }
            if (!string.IsNullOrEmpty(UserSurname))
            {
                string usersurnameLower = UserSurname.ToLower();
                users = users.Where(x => x.UserSurname.ToLower().Contains(usersurnameLower));
            }
            if (!string.IsNullOrEmpty(Password))
            {
                password = (Password);


                users = users.Where(x => x.Password.Equals(password));
            }

            if (!string.IsNullOrEmpty(UserEmail))
            {
                string emailLower = UserEmail.ToLower();
                users = users.Where(x => x.UserEmail.ToLower().Contains(emailLower));
            }

            if (IsAdmin.HasValue)
                users = users.Where(x => x.IsAdmin == IsAdmin.Value);

            return View(users.ToList());
        }
        [HttpPost]
        public IActionResult ToggleAdmin([FromBody] ToggleAdminModel data)
        {
            try
            {
                
                _antiforgery.ValidateRequestAsync(HttpContext).Wait();
            }
            catch
            {
                return BadRequest("CSRF doğrulaması başarısız.");
            }
            var user = db.Users.FirstOrDefault(x => x.UserId == data.UserId);
            if (user != null)
            {
                user.IsAdmin = data.IsAdmin;
                db.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);

            if (user != null)
            {
              
                var relatedEmotes = db.UserNewsEmotes.Where(x => x.UserId == id).ToList();
                db.UserNewsEmotes.RemoveRange(relatedEmotes);

                var relatedComments = db.Comments.Where(x => x.UserId == id).ToList();
                db.Comments.RemoveRange(relatedComments);

                
                db.Users.Remove(user);
                db.SaveChanges();

                TempData["DeleteMessage"] = "Success! User has been deleted!";
            }
            else
            {
                TempData["DeleteMessage"] = "User can't be found.";
            }

            return RedirectToAction("Index");
        }



    }
}
