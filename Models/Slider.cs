using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Slider
{
    public decimal Slideid { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
    [Display(Name = "Image")]
    public string? Imageurl { get; set; }
    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile? ImageFile { get; set; }
}
