using System;
using System.Collections.Generic;

namespace News_Site.Models.Entity;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
