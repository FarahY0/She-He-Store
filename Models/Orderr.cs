using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Orderr
{
    public decimal Orderid { get; set; }
    [Display(Name = "Order Date")]
    public DateTime? Orderdate { get; set; }
    [Display(Name = "Total Amount")]
    public decimal? Totalamount { get; set; }

    public string? Status { get; set; }

    public decimal? Billingid { get; set; }

    public decimal? Userid { get; set; }

    public virtual Billing? Billing { get; set; }

    public virtual ICollection<Itorder> Itorders { get; set; } = new List<Itorder>();

    public virtual Userr? User { get; set; }
}
public enum Status
{
    btn1, btn2, btn3, btn4
}
