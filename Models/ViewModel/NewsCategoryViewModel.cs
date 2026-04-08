using News_Site.Models.Entity;

namespace News_Site.Models.ViewModel
{
    public class NewsCategoryViewModel
    {
        public Category Category { get; set; }
        public List<News> NewsList { get; set; }

        public List<News> SlideNews { get; set; } = new();
        public List<News> CardNews { get; set; } = new();
        public Dictionary<string, string> Currencies { get; set; }
    }
}
