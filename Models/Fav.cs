using System;
using System.Collections.Generic;

namespace HeAndSheStore.Models;

public partial class Fav
{
    public decimal Id { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Productid { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Userr? User { get; set; }
}
