using System;
using System.Collections.Generic;

namespace Caleb_Marks_Assignment_11._1.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string AuthorName { get; set; } = null!;

    public string? Description { get; set; }

    public int Quantity { get; set; }
}
