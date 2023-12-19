using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Cart
{
    public decimal Cartid { get; set; }

    public decimal? Quantity { get; set; }
    [Display(Name = "Order Status")]
    public string? Orderstatus { get; set; }
    [Display(Name = "Total Price")]
    public decimal? Totalprice { get; set; }
    [Display(Name = "Order Date")]
    public DateTime? Orderdate { get; set; }

    public decimal? Productid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Cardid { get; set; }

    public virtual Payment? Card { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Userr? User { get; set; }
}
