using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Category
{
    public decimal Categoryid { get; set; }
    [Display(Name = "Category Name")]
    public string? Categoryname { get; set; }
    [Display(Name = "Image")]
    public string? Imagepath { get; set; }
    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile? ImageFile { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
