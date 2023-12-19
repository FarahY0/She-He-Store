using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Billing
{
    public decimal Id { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Country { get; set; }

    public decimal? Zip { get; set; }
    [Display(Name = "First Name")]
    public string? Fname { get; set; }
    [Display(Name = "Last Name")]
    public string? Lname { get; set; }

    public string? Message { get; set; }

    public decimal? Userid { get; set; }

    public string? Copon { get; set; }

    public virtual ICollection<Orderr> Orderrs { get; set; } = new List<Orderr>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Userr? User { get; set; }
}
