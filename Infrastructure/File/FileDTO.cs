namespace Infrastructure.File;

public class FileDTO
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream Stream { get; set; }
}