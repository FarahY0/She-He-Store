using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Videoservice
{
    public decimal Id { get; set; }
    [Display(Name = "Video")]
    public string? Videourl { get; set; }
    [NotMapped]
    public IFormFile? VideoFile { get; set; }
}
