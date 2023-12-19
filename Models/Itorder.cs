using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Itorder
{
    public decimal Id { get; set; }

    public decimal? Quantity { get; set; }
    [Display(Name = "Item Price")]
    public decimal? Itemprice { get; set; }

    public decimal? Productid { get; set; }

    public decimal? Orderid { get; set; }

    public virtual Orderr? Order { get; set; }

    public virtual Product? Product { get; set; }
}
