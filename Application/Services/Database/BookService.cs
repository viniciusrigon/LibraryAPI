using Application.Interfaces;

using Domain.Entities;

namespace Application.Services;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }


    public async Task<IEnumerable<Book>> Get()
    {
        return await _bookRepository.Get();
    }

    public async Task<Book> Insert(Book book)
    {
        return await _bookRepository.Create(book);
    }

    public async Task<Book> Update(Book book)
    {
        return await _bookRepository.Update(book);
    }

    public async Task Delete(long id)
    {
        await _bookRepository.Delete(id);
    }
}