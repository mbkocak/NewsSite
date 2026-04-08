using System;
using System.Collections.Generic;

namespace News_Site.Models.Entity;

public partial class Action
{
    public int ActionId { get; set; }

    public int UserId { get; set; }

    public int NewsId { get; set; }

    public int EmoteId { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual Emote Emote { get; set; } = null!;

    public virtual News News { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
