using News_Site.Models.Entity;

namespace News_Site.Models.ViewModel
{
    public class NewsViewModel
    {
        public string NewsTitle { get; set; }
        public string NewsDetail { get; set; }
        public string Image { get; set; }
        public string LoadingDate { get; set; }
        public int NewsId { get; set; }
        public string Link { get; set; }
        public string MetaDescription { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }



        public int CommentId { get; set; }
        public string CommentContent { get; set; }

        public IEnumerable<News> News { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
