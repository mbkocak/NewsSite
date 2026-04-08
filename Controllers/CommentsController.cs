using Microsoft.AspNetCore.Mvc;
using News_Site.Models.Entity;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using News_Site.Models.ViewModel;
using Microsoft.Identity.Client.Extensions.Msal;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography.Xml;


namespace News_Site.Controllers
{
    public class CommentsController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();
        List<CommentsViewModel> commentsViewModel = new List<CommentsViewModel>();
        //Listing
        public IActionResult Index(CommentsFilterViewModel filter)
        {

          

            var users = db.Users.ToList();
            var comments = db.Comments.ToList();
            var news = db.News.ToList();

            var query = from Comment in comments
                        join News in news on Comment.NewsId equals News.NewsId
                        join Users in users on Comment.UserId equals Users.UserId

                        select new CommentsViewModel
                        {
                            NewsId = News.NewsId,
                            NewsTitle = News.NewsTitle,
                            NewsDetail = News.NewsDetail,
                            Image = News.Image,

                            CommentId = Comment.CommentId,
                            CommentContent = Comment.CommentContent,
                            LoadingDate = Comment.LoadingDate,

                            UserId = Users.UserId,
                            UserName = Users.UserName,
                            UserSurname = Users.UserSurname,

                        };

            if (!string.IsNullOrEmpty(filter.NewsTitle))
            {

                string lowerFilterTitle = filter.NewsTitle.ToLower();
                query = query.Where(x => x.NewsTitle != null && x.NewsTitle.ToLower().Contains(lowerFilterTitle));

            }
            if (!string.IsNullOrEmpty(filter.NewsDetail))
            {

                string lowerFilterDetail = filter.NewsDetail.ToLower();
                query = query.Where(x => x.NewsDetail != null && x.NewsDetail.ToLower().Contains(lowerFilterDetail));


            }
            if (!string.IsNullOrEmpty(filter.Image))
            {

                query = query.Where(x => x.Image.Contains(filter.Image));

            }

            if (!string.IsNullOrEmpty(filter.CommentContent))
            {

                string lowerFilterComment = filter.CommentContent.ToLower();
                query = query.Where(x => x.CommentContent != null && x.CommentContent.ToLower().Contains(lowerFilterComment));

            }

            if (filter.LoadingDate.HasValue)
                query = query.Where(x => x.LoadingDate.HasValue && x.LoadingDate.Value.Date == filter.LoadingDate.Value.Date);


            if (!string.IsNullOrEmpty(filter.UserName))
            {
                string lowerFilterName = filter.UserName.ToLower();
                query = query.Where(x => x.UserName != null && x.UserName.ToLower().Contains(lowerFilterName));
            }
               

            if (!string.IsNullOrEmpty(filter.UserSurname))
            {
                string lowerFilterSurname = filter.UserSurname.ToLower();
                query = query.Where(x => x.UserSurname != null && x.UserSurname.ToLower().Contains(lowerFilterSurname));
            }
                

            var viewModel = new CommentsIndexViewModel
            {
                Filter = filter,
                Comments = query.ToList()
            };
            return View(viewModel);
        }

        //Add
        [HttpGet]
        public ActionResult NewComment()
        {
            CommentsViewModel commentViewModel = new CommentsViewModel();
            var news = db.News.ToList();
            var users = db.Users.ToList();
            var comments = db.Comments.ToList();

            commentViewModel.News = news;
            commentViewModel.Users = users;
            commentViewModel.Comments = comments;


            return View(commentViewModel);
        }

        [HttpPost]
        public ActionResult NewTransport(News news, User users, Comment comments)
        {

            db.Comments.Add(comments);
            db.SaveChanges();

            TempData["AddMessage"] = "";
            return RedirectToAction("Index");
        }




        //Delete
        public ActionResult Delete(int id)
        {
            var comment = db.Comments.Find(id);

            db.Comments.Remove(comment);
            db.SaveChanges();

            TempData["DeleteMessage"] = "Comment deleted successfully!";
            return RedirectToAction("Index");



        }

    }
}
