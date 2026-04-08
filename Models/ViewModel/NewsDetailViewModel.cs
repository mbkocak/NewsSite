using News_Site.Models.Entity;

namespace News_Site.Models.ViewModel
{
    public class NewsDetailViewModel
    {
        public News News { get; set; }
        public List<Emote> Emotes { get; set; }
        public List<Comment> Comments { get; set; }
        public string NewCommentContent { get; set; }
        public Dictionary<int, int> NewsEmoteCounts { get; set; }
        public List<News> SideNews { get; set; }
        public Dictionary<string, string> Currencies { get; set; }
    }
}
