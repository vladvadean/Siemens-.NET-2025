using System.ComponentModel.DataAnnotations;

namespace Siemens.NET2025.Model;

public class Book
{
    public long Id {get; set;}
    [Required(ErrorMessage = "Title is required.")]
    public string Title {get; set;}
    [Required(ErrorMessage = "Author is required.")]
    public string Author {get; set;}
    [Required(ErrorMessage = "AvailableQuantity is required.")]
    public long AvailableQuantity {get; set;}
    public List<Category> Categories {get; set;}
}