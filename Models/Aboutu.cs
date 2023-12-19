using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Aboutu
{
    public decimal Aboutusid { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }
    [Display(Name = "Image")]
    public string? Imagepath { get; set; }
    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile? ImageFile { get; set; }

    public string? Imagepathtwo { get; set; }

    public decimal? Description { get; set; }
}
