using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;
    public FilesController(AppConfiguration appConfiguration)
    {
        _s3Client = new AmazonS3Client(appConfiguration.AwsAccessKey, appConfiguration.AwsSecretAccessKey, RegionEndpoint.USEast2);
    }
    
    // GET
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
    {
        
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
        var request = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
            InputStream = file.OpenReadStream()
        };
        request.Metadata.Add("Content-Type", file.ContentType);
        await _s3Client.PutObjectAsync(request);
        return Ok($"File {prefix}/{file.FileName} uploaded to S3 successfully!");
    }
}