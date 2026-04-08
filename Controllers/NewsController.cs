using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News_Site.Models.Entity;
using System.Security.Cryptography.Xml;
using System.Diagnostics;
using Microsoft.Identity.Client;
using News_Site.Models.ViewModel;
using Microsoft.Identity.Client.Extensions.Msal;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using News_Site.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using News_Site.Models.Services;
namespace News_Site.Controllers
{
   
    public class NewsController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();
        List<NewsViewModel> NewsViewModel = new List<NewsViewModel>();

        //Listing
        public IActionResult Index(string NewsTitle, string NewsDetail, string Image, string LoadingDate, string Link, string MetaDescription, int CategoryId, string CategoryName, int CommentId, string CommentContent, int NewsId)
        {
            int newsid;
            string newstitle;
            string newsdetail;
            string image;
            string loadingdate;
            string link;
            string metadescription;

            int categoryid;
            string categoryname;

            int commentid;
            string commentcontent;

            var news = db.News.ToList();
            var categories = db.Categories.ToList();
            var comments = db.Comments.ToList();

            var query = from News in news
                        join Category in categories on News.CategoryId equals Category.CategoryId
                        join Comment in comments on News.NewsId equals Comment.NewsId into commentGroup
                        from Comment in commentGroup.DefaultIfEmpty()

                        select new NewsViewModel
                        {
                            NewsId = News.NewsId,
                            NewsTitle = News.NewsTitle,
                            NewsDetail = News.NewsDetail,
                            Image = News.Image,
                            LoadingDate = News.LoadingDate,
                            Link = News.Link,
                            MetaDescription = News.MetaDescription,

                            CommentId = Comment?.CommentId ?? 0,
                            CommentContent = Comment?.CommentContent ?? "",


                            CategoryId = Category.CategoryId,
                            CategoryName = Category.CategoryName,

                        };

            if (!string.IsNullOrEmpty(NewsTitle))
            {
                newstitle = NewsTitle.ToLower();
                query = query.Where(x => x.NewsTitle != null && x.NewsTitle.ToLower().Contains(newstitle));
            }
            if (!string.IsNullOrEmpty(NewsDetail))
            {

                query = query.Where(x => x.NewsDetail.Equals(NewsDetail));

            }
            if (!string.IsNullOrEmpty(Image))
            {

                image = Image;
                query = query.Where(x => x.Image.Equals(image));

            }

            if (!string.IsNullOrEmpty(LoadingDate))
            {
                loadingdate = LoadingDate;
                query = query.Where(x => x.LoadingDate.Equals(loadingdate));
            }
            if (!string.IsNullOrEmpty(Link))
            {
                link = Link;
                query = query.Where(x => x.Link.Equals(link));

            }
            if (!string.IsNullOrEmpty(MetaDescription))
            {

                metadescription = MetaDescription.ToLower();
                query = query.Where(x => x.NewsTitle != null && x.MetaDescription.ToLower().Contains(metadescription));

            }
            if (!string.IsNullOrEmpty(CategoryName))
            {

                categoryname = CategoryName;
                query = query.Where(x => x.CategoryName.Equals(categoryname));

            }
            if (!string.IsNullOrEmpty(CommentContent))
            {

                commentcontent = CommentContent;
                query = query.Where(x => x.CommentContent.Equals(commentcontent));

            }

            NewsViewModel = query.ToList();
            return View(NewsViewModel);

        }
        // Add
        [HttpGet]
        public ActionResult AddNews()
        {
            NewsAndCategoryViewModel newsAndCategoryViewModel = new NewsAndCategoryViewModel();
            var news = db.News.ToList();
            var categories = db.Categories.ToList();


            newsAndCategoryViewModel.News = news;
            newsAndCategoryViewModel.Categories = categories;


            return View(newsAndCategoryViewModel);

        }

        [HttpPost]
        public ActionResult AddNews(News news, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }


                news.Image = "/images/" + fileName;
            }
            news.Link = SeoHelper.ToSeoUrl(news.NewsTitle);

            if (news.CategoryId == 0)
            {
                news.CategoryId = 1;
            }

            db.News.Add(news);
            db.SaveChanges();
            TempData["AddMessage"] = "News added successfully!";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var news = db.News
                .Include(n => n.NewsEmote)
                .Include(n => n.Comments)
                .Include(n => n.Images)
                .Include(n => n.Actions) 
                .FirstOrDefault(n => n.NewsId == id);

            if (news == null)
                return NotFound();

           
            var userNewsEmotes = db.UserNewsEmotes.Where(u => u.NewsId == id);
            db.UserNewsEmotes.RemoveRange(userNewsEmotes);

            db.NewsEmote.RemoveRange(news.NewsEmote);
            db.Comments.RemoveRange(news.Comments);
            db.Images.RemoveRange(news.Images);
            db.Actions.RemoveRange(news.Actions); 

            db.News.Remove(news);
            db.SaveChanges();

            TempData["DeleteMessage"] = "News deleted successfully!";

            return RedirectToAction("Index");
        }





        //Update
        public ActionResult Update(int id)
        {
            var news = db.News.Find(id);
            var categories = db.Categories.ToList();
            var comments = db.Comments.ToList();

            NewsUpdateViewModel viewModel = new NewsUpdateViewModel
            {
                News = news,
                Categories = categories,
                Comments = comments
            };



            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ApplyUpdate(NewsUpdateViewModel model, IFormFile ImageFile)
        {
            var nws = db.News.Find(model.News.NewsId);
            if (nws != null)
            {
                nws.NewsTitle = model.News.NewsTitle;
                nws.NewsDetail = model.News.NewsDetail;
                nws.LoadingDate = model.News.LoadingDate;
                nws.Link = model.News.Link;
                nws.MetaDescription = model.News.MetaDescription;
                nws.CategoryId = model.News.CategoryId;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    nws.Image = "/images/" + fileName;
                }

                db.News.Update(nws);
                db.SaveChanges();
                TempData["UpdateMessage"] = "News updated!";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            var news = db.News.FirstOrDefault(x => x.Link == id);
            if (news == null) return NotFound();

            var emotes = db.Emotes.ToList();
            var comments = db.Comments
                .Where(c => c.NewsId == news.NewsId)
                .Include(c => c.User)
                .ToList();

            var newsEmoteCounts = db.NewsEmote
                .Where(ne => ne.NewsId == news.NewsId)
                .ToDictionary(ne => ne.EmoteId, ne => ne.ClickCount);

            var sideNews = db.News
        .Where(n => n.NewsId != news.NewsId)
        .OrderByDescending(n => n.LoadingDate)
        .Take(6)
        .ToList();


            foreach (var emote in emotes)
            {
                if (!newsEmoteCounts.ContainsKey(emote.EmoteId))
                {
                    newsEmoteCounts[emote.EmoteId] = 0;
                }
            }

            var currencyService = new CurrencyService();
            var currencies = currencyService.GetCurrencies();

            var viewModel = new NewsDetailViewModel
            {
                News = news,
                Emotes = emotes,
                Comments = comments,
                NewsEmoteCounts = newsEmoteCounts,
                SideNews = sideNews,
                Currencies = currencies
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Click([FromBody] ClickRequest request)
        {

            var userName = User.Identity?.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == userName);
           
            var existingVote = db.UserNewsEmotes
                .FirstOrDefault(une => une.UserId == user.UserId && une.NewsId == request.NewsId);

            if (existingVote != null)
            {
                
                var oldEmote = db.NewsEmote
                    .FirstOrDefault(ne => ne.NewsId == request.NewsId && ne.EmoteId == existingVote.EmoteId);
                if (oldEmote != null && oldEmote.ClickCount > 0)
                {
                    oldEmote.ClickCount--;
                }

                
                db.UserNewsEmotes.Remove(existingVote);
            }

            
            var newEmote = db.NewsEmote
                .FirstOrDefault(ne => ne.NewsId == request.NewsId && ne.EmoteId == request.EmoteId);

            if (newEmote == null)
            {
                newEmote = new NewsEmote
                {
                    NewsId = request.NewsId,
                    EmoteId = request.EmoteId,
                    ClickCount = 1
                };
                db.NewsEmote.Add(newEmote);
            }
            else
            {
                newEmote.ClickCount++;
            }

            var newUserVote = new UserNewsEmote
            {
                UserId = user.UserId,
                NewsId = request.NewsId,
                EmoteId = request.EmoteId,
                ClickDate = DateTime.Now
            };
            db.UserNewsEmotes.Add(newUserVote);

            db.SaveChanges();
            return Ok("Tıklama güncellendi.");
        }

        public class ClickRequest
        {
            public int NewsId { get; set; }
            public int EmoteId { get; set; }
        }


        [HttpPost]
        [Authorize]
        public IActionResult AddCommentAjax([FromBody] CommentRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || string.IsNullOrWhiteSpace(request.CommentContent))
            {
                return BadRequest("Yorum boş olamaz.");
            }

            int userId = int.Parse(userIdClaim.Value);

            var comment = new Comment
            {
                NewsId = request.NewsId,
                CommentContent = request.CommentContent,
                LoadingDate = DateTime.Now,
                UserId = userId
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            var user = db.Users.FirstOrDefault(u => u.UserId == userId);

            return Json(new
            {
                success = true,
                userName = user?.UserName,
                userSurname = user?.UserSurname,
                content = comment.CommentContent,
                date = comment.LoadingDate?.ToString("dd.MM.yyyy HH:mm")
            });
        }

        public class CommentRequest
        {
            public int NewsId { get; set; }
            public string CommentContent { get; set; }
        }
 


        [HttpGet("News/Category/{categoryName}")]
        public IActionResult CategoryDetails(string categoryName)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
            if (category == null)
                return NotFound();

            var newsList = db.News.Where(n => n.CategoryId == category.CategoryId).ToList();
            var currencyService = new CurrencyService();
            var currencies = currencyService.GetCurrencies();

            var model = new NewsCategoryViewModel
            {
                Category = category,
                NewsList = newsList,
                Currencies = currencies
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index", "Home");

            var matchedNews = db.News
                .Where(n => n.NewsTitle.ToLower().Contains(query.ToLower()))
                .ToList();

            var otherNews = db.News
    .OrderBy(n => Guid.NewGuid())
    .ToList();

            var currencyService = new CurrencyService();
            var currencies = currencyService.GetCurrencies();

            var model = new SearchViewModel
            {
                NewsResults = matchedNews,
                NotFoundMessage = matchedNews.Any() ? null : "Aradığınız başlığa uygun haber bulunamadı. Farklı bir terim girerek aramanızı tekrarlayabilirsiniz ya da kategorilerimize göz atabilirsiniz.",
                Currencies = currencies,
                OtherNews = otherNews
            };

            return View("Search", model);
        }






    }
}
