using System;
using System.Collections.Generic;

namespace News_Site.Models.Entity;

public partial class Image
{
    public int ImageId { get; set; }

    public int NewsId { get; set; }

    public string ImagePath { get; set; } = null!;

    public DateTime? AddDate { get; set; }

    public virtual News News { get; set; } = null!;
}
