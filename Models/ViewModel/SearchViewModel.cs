using News_Site.Models.Entity;

namespace News_Site.Models.ViewModel
{
    public class SearchViewModel
    {
        public List<News> NewsResults { get; set; }
        public string? NotFoundMessage { get; set; }

        public Dictionary<string, string> Currencies { get; set; }
        public List<News>? OtherNews { get; set; }
    }
}
