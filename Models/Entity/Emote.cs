using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace News_Site.Models.Entity;

public partial class Emote
{
    public int EmoteId { get; set; }

    public string EmoteName { get; set; } = null!;

    public string EmotePath { get; set; } = null!;

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();
    public virtual ICollection<NewsEmote> NewsEmote { get; set; } = new List<NewsEmote>();
}
