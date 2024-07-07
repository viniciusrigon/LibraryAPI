using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Domain.Entities;
using Application.Services;
using AutoMapper;
using Infrastructure.File;
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
    private readonly AWSService _awsService;
    private readonly IMapper _mapper;
    private readonly string _cloudFromDomainName;

    public LibraryController(ILogger<LibraryController> logger,
        BookService bookService,
        IMapper mapper,
        AWSService awsService,
        AppConfiguration appConfiguration)
    {
        _logger = logger;
        _bookService = bookService;
        _mapper = mapper;
        _awsService = awsService;
        _cloudFromDomainName = appConfiguration.CloudFrontDomainName;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> Get()
    {
        try
        {
            var results = await _bookService.Get();
            var books = _mapper.Map<IEnumerable<BookResponseDto>>(results);
            return Ok(books);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet]
    [Route("GetBookById")]
    public async Task<ActionResult<BookResponseDto>> GetBook(long id)
    {
        try
        {
            var result = await _bookService.Get(id);
            var books = _mapper.Map<BookResponseDto>(result);
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
            var uploadReponse = await _awsService.UploadFile(new FileDTO()
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Stream = file.OpenReadStream()
            });

            if (uploadReponse.HttpStatusCode == HttpStatusCode.OK)
            {

                var result = await _bookService.Get(bookId);
                var book = _mapper.Map<BookResponseDto>(result);
                book.CoverUrl = $"{_cloudFromDomainName}/{file.FileName}";

                var updateBook = _mapper.Map<Book>(book);
                var resultUpdate = await _bookService.Update(updateBook);

                return Ok();
            }
            else
            {
                return BadRequest(uploadReponse);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Post([FromBody] BookRequestDto book)
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
    [Route("{bookId:long}")]
    public async Task<ActionResult<BookResponseDto>> Put(long bookId, [FromBody] BookRequestDto bookBody)
    {
        try
        {
            var bookEntity = await _bookService.Get(bookId);
            bookEntity.ReleaseYear = bookBody.ReleaseYear;
            bookEntity.Author = bookBody.Author;
            bookEntity.Title = bookBody.Title;
            
            var result = await _bookService.Update(bookEntity);
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