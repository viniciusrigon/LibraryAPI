using Domain.Entities;
using Domain.Services;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController : ControllerBase
{
    private readonly ILogger<LibraryController> _logger;
    private readonly BookService _bookService;

    public LibraryController(ILogger<LibraryController> logger, BookService bookService)
    {
        _logger = logger;
        _bookService = bookService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<Book> Get()
    {
        return new List<Book>();
    }

    [HttpPost]
    public ActionResult<BookModel> PostBook(BookModel book)
    {
        throw new NotImplementedException();
    }
}