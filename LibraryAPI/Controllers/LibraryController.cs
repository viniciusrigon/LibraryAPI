using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Domain.Entities;
using Application.Services;
using AutoMapper;
using Infrastructure.Storage;
using LibraryAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly ILogger<LibraryController> _logger;
    private readonly BookService _bookService;
    private readonly IMapper _mapper;
    private readonly IAmazonS3 _s3Client;
    private readonly string BUCKET_NAME;
    private readonly string _cloudFromDomainName;
    private const string prefix = "covers";

    public LibraryController(ILogger<LibraryController> logger, BookService bookService, IMapper mapper, AppConfiguration appConfiguration)
    {
        _logger = logger;
        _bookService = bookService;
        _mapper = mapper;
        _s3Client = new AmazonS3Client(appConfiguration.AwsAccessKey, appConfiguration.AwsSecretAccessKey, RegionEndpoint.USEast2);
        BUCKET_NAME = appConfiguration.BucketName;
        _cloudFromDomainName = appConfiguration.CloudFrontDomainName;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDTO>>> Get()
    {
        try
        {
            var results = await _bookService.Get();
            var books = _mapper.Map<IEnumerable<BookDTO>>(results);
            return Ok(books);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet]
    [Route("GetBookById")]
    public async Task<ActionResult<BookDTO>> GetBook(long id)
    {
        try
        {
            var result = await _bookService.Get(id);
            var books = _mapper.Map<BookDTO>(result);
            return Ok(books);
            
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost]
    [Route("{bookId:long}/uploadcover")]
    public async Task<IActionResult> UploadFileAsync(long bookId, IFormFile file)
    {
        try
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(BUCKET_NAME);
            if (!bucketExists) return NotFound($"Bucket {BUCKET_NAME} does not exist.");
            var request = new PutObjectRequest()
            {
                BucketName = BUCKET_NAME,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _s3Client.PutObjectAsync(request);

            // var urlRequest = new GetPreSignedUrlRequest()
            // {
            //     BucketName = BUCKET_NAME,
            //     Key = request.Key,
            //     Expires = DateTime.UtcNow.AddHours(1)
            // };
            //
            // string coverUrl = await _s3Client.GetPreSignedURLAsync(urlRequest);
        
            var result = await _bookService.Get(bookId);
            var book = _mapper.Map<BookDTO>(result);
            book.CoverUrl = $"{_cloudFromDomainName}/{file.FileName}";
        
            var updateBook = _mapper.Map<Book>(book);
            var resultUpdate = await _bookService.Update(updateBook);
        
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost]
    public async Task<ActionResult<BookDTO>> Post([FromBody] BookDTO book)
    {
        try
        {
            var newBook = _mapper.Map<Book>(book);
            var results = await _bookService.Insert(newBook);
            return Ok(results);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut]
    public async Task<ActionResult<BookDTO>> Put([FromBody] BookDTO book)
    {
        try
        {
            var updateBook = _mapper.Map<Book>(book);
            var result = await _bookService.Update(updateBook);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(long id)
    {
        try
        {
            await _bookService.Delete(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        
    }
    
    
}