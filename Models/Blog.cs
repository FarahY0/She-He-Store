using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeAndSheStore.Models;

public partial class Blog
{
    public decimal Blogid { get; set; }

    public string? Blogtitle { get; set; }

    public string? Description { get; set; }

    public DateTime? Blogdate { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public decimal? Userid { get; set; }

    public virtual Userr? User { get; set; }
}
