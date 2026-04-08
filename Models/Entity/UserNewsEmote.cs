using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace News_Site.Models.Entity

{
    [Table("UserNewsEmote")]
    public class UserNewsEmote
    {
        [Key]
        [Column("user_news_emote_id")]
        public int UserNewsEmoteId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Column("news_id")]
        public int NewsId { get; set; }
        public virtual News News { get; set; }

        [Column("emote_id")]
        public int EmoteId { get; set; }
        public virtual Emote Emote { get; set; }

        [Column("click_date")]
        public DateTime ClickDate { get; set; }
    }
}
