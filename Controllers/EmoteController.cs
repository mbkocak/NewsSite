using Microsoft.AspNetCore.Mvc;
using News_Site.Models.Entity;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Identity.Client;
using News_Site.Models.ViewModel;
namespace News_Site.Controllers
{
    public class EmoteController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();
        public IActionResult Index(int EmoteId, string EmoteName, string EmotePath)
        {
            int emoteid;
            string emotename;
            string emotepath;
            var emotes = db.Emotes.AsQueryable();

            if (!string.IsNullOrEmpty(EmoteName))
            {
                string emotenameLower = EmoteName.ToLower();
                emotes = emotes.Where(x => x.EmoteName.ToLower().Contains(emotenameLower));
            }

            if (!string.IsNullOrEmpty(EmotePath))
            {
                string emotepathLower = EmotePath.ToLower();
                emotes = emotes.Where(x => x.EmotePath.ToLower().Contains(emotepathLower));
            }

            return View(emotes);
        }

        //Add
        [HttpGet]
        public ActionResult AddEmote()
        {

            var emotes = db.Emotes.ToList();
            return View(emotes);
        }



        [HttpPost]
        public ActionResult AddEmote(Emote emote, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                emote.EmotePath = "/images/" + fileName;
            }

            if (string.IsNullOrEmpty(emote.EmoteName))
            {
                ModelState.AddModelError("", "Emote name cannot be empty.");
                return View();
            }

            try
            {
                db.Emotes.Add(emote);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message); 
                throw;
            }
            TempData["AddMessage"] = "Emote added successfully!";
            return RedirectToAction("Index");
        }



        //Delete
        public ActionResult Delete(int id)
        {
            var emote = db.Emotes.Find(id);

            db.Emotes.Remove(emote);
            db.SaveChanges();

            TempData["DeleteMessage"] = "Emote deleted successfully!";
            return RedirectToAction("Index");

        }

        //Update
        public ActionResult Update(int id)
        {
            var emt = db.Emotes.Find(id);
            return View(emt);

        }

        [HttpPost]
        public ActionResult ApplyUpdate(Emote emote, IFormFile ImageFile)
        {
            var emt = db.Emotes.Find(emote.EmoteId);
            if (emt != null)
            {
                emt.EmoteName = emote.EmoteName;
              
               

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    emt.EmotePath = "/images/" + fileName;
                }

                db.Emotes.Update(emt);
                db.SaveChanges();
                TempData["UpdateMessage"] = "Emote updated successfully!";
            }

            return RedirectToAction("Index");
        }
    }
}
