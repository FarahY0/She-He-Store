using System;
using System.Collections.Generic;

namespace HeAndSheStore.Models;

public partial class Notification
{
    public decimal Notificationid { get; set; }

    public decimal? Userid { get; set; }

    public string? Message { get; set; }

    public DateTime? Datesent { get; set; }

    public virtual Userr? User { get; set; }
}
