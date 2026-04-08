using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using News_Site.Models.Entity;
using News_Site.Models.ViewModel;

namespace News_Site.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();

        public IActionResult Index()
        {
            // Admin / User oranı
            var roleDistributions = db.Users
                .GroupBy(u => u.IsAdmin ? "Admin" : "User")
                .Select(g => new RoleDistribution
                {
                    RoleName = g.Key,
                    Count = g.Count()
                }).ToList();

            // Günlük haber sayısı
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-6);

            var newsList = db.News.ToList(); // Haberleri belleğe al (çünkü LoadingDate string)

            var dailyNewsCounts = newsList
                .Where(n => DateTime.Parse(n.LoadingDate) >= sevenDaysAgo)
                .GroupBy(n => DateTime.Parse(n.LoadingDate).Date)
                .Select(g => new NewsDailyStat
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Count()
                }).ToList();

            var allDays = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-i).ToString("yyyy-MM-dd"))
                .OrderBy(d => d)
                .ToList();

            var completeDailyStats = allDays.Select(date => new NewsDailyStat
            {
                Date = date,
                Count = dailyNewsCounts.FirstOrDefault(x => x.Date == date)?.Count ?? 0
            }).ToList();

            // ViewModel oluştur
            var model = new AdminDashboardViewModel
            {
                TotalUsers = db.Users.Count(),
                TotalNews = db.News.Count(),
                TotalCategories = db.Categories.Count(),

                CategoryCounts = db.Categories
                    .Select(c => new CategoryNewsCount
                    {
                        CategoryName = c.CategoryName,
                        NewsCount = c.News.Count()
                    }).ToList(),

                RoleDistributions = roleDistributions,
                DailyNewsCounts = completeDailyStats
            };

            return View(model);
        }
    }
}
