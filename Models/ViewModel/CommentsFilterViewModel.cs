namespace News_Site.Models.ViewModel
{
    public class CommentsFilterViewModel
    {
        public string NewsTitle { get; set; }
        public string NewsDetail { get; set; }
        public string Image { get; set; }
        public string CommentContent { get; set; }
        public DateTime? LoadingDate { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
    }
    public class CommentsIndexViewModel
    {
        public CommentsFilterViewModel Filter { get; set; } = new CommentsFilterViewModel();
        public IEnumerable<CommentsViewModel> Comments { get; set; }
    }

}
