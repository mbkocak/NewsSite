using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using News_Site.Models;
using News_Site.Models.Entity;
using News_Site.Models.ViewModel;
using System.Linq;
using News_Site.Models.Services;

namespace News_Site.Controllers
{
    public class HomeController : Controller
    {
        NewsSiteContext db = new NewsSiteContext();
        List<HomePageViewModel> NewsViewModel = new List<HomePageViewModel>();
        private readonly ILogger<HomeController> _logger;
        private readonly NewsSiteContext _context;

        public HomeController(ILogger<HomeController> logger, NewsSiteContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var currencyService = new CurrencyService();
            var currencies = currencyService.GetCurrencies();

            var model = new HomePageViewModel
            {
                SlideNews = _context.News.Take(10).ToList(),
                CardNews = _context.News.Skip(4).Take(11).ToList(),
                SidebarNews = _context.News.OrderByDescending(n => n.LoadingDate).Take(5).ToList(),
                Currencies = currencies
            };

            return View(model);
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
