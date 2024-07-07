using System.Text.Json.Serialization;

namespace LibraryAPI.DTO;

public partial class BookRequestDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int ReleaseYear { get; set; }
    public string CoverUrl { get; set; }
}