using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Service
{
    public decimal Serviceid { get; set; }
    [Display(Name = "Service Name")]
    public string? Servicename { get; set; }

    public string? Description { get; set; }

    public string? Videourl { get; set; }
    [Display(Name = "Image")]
    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
