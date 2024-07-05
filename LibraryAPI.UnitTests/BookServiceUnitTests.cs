using Application.Interfaces;
using Application.Services;
using Bogus;
using Domain.Entities;
using Moq;

namespace LibraryAPI.Tests;

[TestFixture]
public class BookServiceUnitTests
{
    private BookService _bookService;
    private Faker<Book> _booksMock;


    [SetUp]
    public void Setup()
    {
        int _id = 0;

        this._booksMock = new Faker<Book>();

        var bookRepositoryMock = new Mock<IBookRepository>();

        _booksMock
            .RuleFor(x => x.Id, ++_id)
            .RuleFor(x => x.ReleaseYear, f => f.Date.Past(10).Year)
            .RuleFor(x => x.Author, f => f.Person.FullName)
            .RuleFor(x => x.Title, f => f.Lorem.Sentence(4))
            .RuleFor(x => x.CoverUrl, f => f.Image.PicsumUrl());

        var mockData = _booksMock.GenerateBetween(1, 20).AsEnumerable<Book>();

        bookRepositoryMock.Setup(c => c.Get()).ReturnsAsync(mockData);
        //bookRepositoryMock.Setup(c => c.GetById(booksMock.RuleFor(x =>x.Id, f => f.Random.Long(1, 20))))!.ReturnsAsync(mockData.FirstOrDefault());
        bookRepositoryMock.Setup(c => c.Create(It.IsAny<Book>())).ReturnsAsync(_booksMock);
        bookRepositoryMock.Setup(c => c.Update(It.IsAny<Book>())).ReturnsAsync(_booksMock);
        //bookRepositoryMock.Setup(c => c.Delete()).ReturnsAsync(booksMock);

        _bookService = new BookService(bookRepositoryMock.Object);

    }

    [Test]
    public void Get()
    {
        var actual = _bookService.Get();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Result.AsEnumerable().Count() > 0);
    }

    [Test]
    public void Create()
    {
        var book = _bookService.Insert(_booksMock);
        
        Assert.IsNotNull(book);
        Assert.AreEqual(book.Result.Id, 1);
    }
    
    [Test]
    public void Update()
    {
        var book = _bookService.Update(_booksMock);
        
        Assert.IsNotNull(book);
        Assert.AreEqual(book.Result.Id, 1);
    }
}