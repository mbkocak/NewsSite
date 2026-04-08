using News_Site.Models.Entity;
using System.Collections.Generic;
namespace News_Site.Models.ViewModel
{
    public class HomePageViewModel
    {
        public List<News> SlideNews { get; set; } = new();
        public List<News> CardNews { get; set; } = new();
        public List<News> NewsList { get; set; }
        public List<Category> Categories { get; set; }
        public List<News> SidebarNews { get; set; } = new();
        public Dictionary<string, string> Currencies { get; set; }
    }
}
