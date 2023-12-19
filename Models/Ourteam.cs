using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Ourteam
{
    public decimal Id { get; set; }
    [Display(Name = "Image")]
    public string? Imagepath { get; set; }
    [Display(Name = "Employee Name")]
    public string? Empname { get; set; }
    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile? ImageFile { get; set; }
    [Display(Name = "Employee position")]
    public string? Empposition { get; set; }
    [Display(Name = "Linkedin Account")]
    public string? Emplinkedin { get; set; }

    public string? Empgmail { get; set; }
    [Display(Name = "instagram Account")]
    public string? Empinstagram { get; set; }
}
