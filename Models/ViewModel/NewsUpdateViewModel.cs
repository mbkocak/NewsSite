using Microsoft.Identity.Client.Extensions.Msal;
using System.Security.Cryptography.Xml;
using News_Site.Models.Entity;
namespace News_Site.Models.ViewModel
{
    public class NewsUpdateViewModel
    {
        public News News { get; set; } // Tekil News nesnesi
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

    }
}
