using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Review
{
    public decimal Reviewid { get; set; }

    public decimal? Rating { get; set; }

    public string? Comments { get; set; }
    [Display(Name = "Review Date")]
    public DateTime? Reviewdate { get; set; }

    public decimal? Productid { get; set; }

    public decimal? Userid { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Userr? User { get; set; }
}
