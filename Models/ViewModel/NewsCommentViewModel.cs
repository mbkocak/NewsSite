using News_Site.Models.Entity;

namespace News_Site.Models.ViewModel
{
    public class NewsCommentViewModel
    {
        public News News { get; set; }
        public List<Emote> Emotes { get; set; }
        public List<Comment> Comments { get; set; }
        public string NewCommentContent { get; set; }
    }
}
