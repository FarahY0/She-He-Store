using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Product
{
    public decimal Productid { get; set; }
    [Display(Name = "Product Name")]
    public string? Productname { get; set; }

    public decimal? Sale { get; set; }

    public decimal? Price { get; set; }

    public string Status { get; set; } = null!;

    public decimal? CategoryId { get; set; }
    [Display(Name = "Image")]
    public string? Imagepath { get; set; }
    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile? ImageFile { get; set; }
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Fav> Favs { get; set; } = new List<Fav>();

    public virtual ICollection<Itorder> Itorders { get; set; } = new List<Itorder>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
