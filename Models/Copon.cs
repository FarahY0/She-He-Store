using System;
using System.Collections.Generic;

namespace HeAndSheStore.Models;

public partial class Copon
{
    public decimal Id { get; set; }

    public string? Copontext { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public decimal? Discoundvalue { get; set; }
}
