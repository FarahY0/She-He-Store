using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Contactu
{
    public decimal Contactusid { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Maplocation { get; set; }
    [Display(Name = "Image")]
    public string? Imagepath { get; set; }
    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile? ImageFile { get; set; }
    public string? Subject { get; set; }

    public string? Message { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }
}
