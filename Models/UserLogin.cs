using System;
using System.Collections.Generic;

namespace HeAndSheStore.Models;

public partial class UserLogin
{
    public decimal Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Roleid { get; set; }

    public string? Newpassword { get; set; }

    public string? Confirmnewpassword { get; set; }

    public decimal? Rememberme { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Userr? User { get; set; }
}
