using System;
using System.Collections.Generic;

namespace News_Site.Models.Entity;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserSurname { get; set; }

    public string? Password { get; set; }

    public string? UserEmail { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
