using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Role
{
    public decimal Roleid { get; set; }
    [Display(Name = "Role Name")]
    public string? Rolename { get; set; }

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    public virtual ICollection<Userr> Userrs { get; set; } = new List<Userr>();
}
