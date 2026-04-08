using System;
using System.Collections.Generic;

namespace News_Site.Models.Entity;

public partial class Comment
{
    public int CommentId { get; set; }

    public int NewsId { get; set; }

    public string CommentContent { get; set; } = null!;

    public DateTime? LoadingDate { get; set; }

    public int? UserId { get; set; }

    public virtual News News { get; set; } = null!;

    public virtual User? User { get; set; }
}
