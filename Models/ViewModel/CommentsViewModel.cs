using Microsoft.Identity.Client.Extensions.Msal;
using System.Security.Cryptography.Xml;
using News_Site.Models.Entity;
namespace News_Site.Models.ViewModel

{
    public class CommentsViewModel
    {

        public string NewsTitle { get; set; }
        public string NewsDetail { get; set; }
        public string Image { get; set; }
        public int NewsId { get; set; }

        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public DateTime? LoadingDate { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public List<News> News { get; set; }
        public List<Comment> Comments { get; set; }
        public List<User> Users { get; set; }

    }

}
