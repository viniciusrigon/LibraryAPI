using Domain.Entities;
using Application.Services;
using AutoMapper;
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

    public LibraryController(ILogger<LibraryController> logger, BookService bookService, IMapper mapper)
    {
        _logger = logger;
        _bookService = bookService;
        _mapper = mapper;
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