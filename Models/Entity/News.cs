using System;
using System.Collections.Generic;

namespace News_Site.Models.Entity;

public partial class News
{
    public int NewsId { get; set; }

    public string? NewsTitle { get; set; }

    public string? NewsDetail { get; set; }

    public string? Image { get; set; }

    public string? LoadingDate { get; set; }

    public string? Link { get; set; }

    public string? MetaDescription { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    public virtual ICollection<NewsEmote> NewsEmote { get; set; } = new List<NewsEmote>();
}
