using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeAndSheStore.Models;

public partial class Testimonial
{
    public decimal TestimonialId { get; set; }

    public string? Comments { get; set; }

    public decimal? Userid { get; set; }

    public bool? Isapproved { get; set; }
    [Display(Name = "Created Date")]
    public DateTime? Createdate { get; set; }

    public string? Status { get; set; }

    public virtual Userr? User { get; set; }
}
