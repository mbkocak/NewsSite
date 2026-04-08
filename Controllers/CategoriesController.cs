using Microsoft.AspNetCore.Mvc;
using News_Site.Models.Entity;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Identity.Client;



namespace News_Site.Controllers
{
    public class CategoriesController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();

        public IActionResult Index(int CategoryId, string CategoryName, string CategoryDescription)
        {
            int categoryid;
            string categoryname;
            string categorydescription;
            var categories = db.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(CategoryName))
            {
                string categorynameLower = CategoryName.ToLower();
                categories = categories.Where(x => x.CategoryName.ToLower().Contains(categorynameLower));
            }

            if (!string.IsNullOrEmpty(CategoryDescription))
            {
                string descriptionLower = CategoryDescription.ToLower();
                categories = categories.Where(x => x.CategoryDescription.ToLower().Contains(descriptionLower));
            }

            return View(categories);
        }

        //Add
        [HttpGet]
        public ActionResult AddCategory()
        {

            var categories = db.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["AddMessage"] = "New category added successfully!";
            }
           
            return RedirectToAction("Index");

        }

        //Delete

        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);

            if (category !=null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                TempData["DeleteMessage"] = "Success! User has been deleted!";
            }

            else
            {
                TempData["DeleteMessage"] = "User can't be found.";
            }

            return RedirectToAction("Index");

        }

        //Update
        public ActionResult Update(int id)
        {
            var ctgr = db.Categories.Find(id);
            return View(ctgr);

        }

        [HttpPost]
        public ActionResult ApplyUpdate(Category category)
        {
            if (ModelState.IsValid)
            {
                var ctgr = db.Categories.Find(category.CategoryId);
                if (category != null)
                {
                    ctgr.CategoryName = category.CategoryName;
                    ctgr.CategoryDescription = category.CategoryDescription;

                    db.Categories.Update(ctgr);
                    db.SaveChanges();
                    TempData["UpdateMessage"] = "Updated successfully!";
                }
            }
                return RedirectToAction("Index");
        }



    }
}
