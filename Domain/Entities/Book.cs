using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int ReleaseYear { get; set; }
    public string CoverUrl { get; set; }
}