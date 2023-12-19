using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Payment
{
    public decimal Cardid { get; set; }
    [Display(Name = "Card number")]
    public decimal Cardnumber { get; set; }

    public decimal? Balance { get; set; }
    [Display(Name = "Card holder name")]
    public string? Cardholdername { get; set; }
    [Display(Name = "Expiration date")]
    public DateTime? Expirationdate { get; set; }

    public string? Cvv { get; set; }
    [Display(Name = "Card type")]
    public string? Cardtype { get; set; }

    public decimal? Userid { get; set; }
    [Display(Name = "Payment Date")]
    public DateTime? Paymentdate { get; set; }

    public decimal? Billingid { get; set; }

    public decimal? Cartid { get; set; }

    public decimal? Productid { get; set; }

    public virtual Billing? Billing { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Product? Product { get; set; }

    public virtual Userr? User { get; set; }
}
