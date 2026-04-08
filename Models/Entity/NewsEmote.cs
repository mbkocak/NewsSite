using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News_Site.Models.Entity
{
    public class NewsEmote
    {
        [Key]
        [Column("news_emote_id")]
        public int NewsEmoteId { get; set; }

        [Column("news_id")]
        public int NewsId { get; set; }

        [ForeignKey(nameof(NewsId))] // doğru kullanım bu
        public virtual News News { get; set; }

        [Column("emote_id")]
        public int EmoteId { get; set; }

        [ForeignKey(nameof(EmoteId))] // doğru kullanım bu
        public virtual Emote Emote { get; set; }

        [Column("click_count")]
        public int ClickCount { get; set; }
    }
}
