namespace LibraryAPI.DTO;

public partial class BookDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int ReleaseYear { get; set; }
    public string CoverUrl { get; set; }
}