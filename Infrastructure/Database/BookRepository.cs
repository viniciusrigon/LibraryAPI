using System.Data;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _libraryContext;

    public BookRepository(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public async Task<IEnumerable<Book>> Get()
    {
        return await _libraryContext.Books.ToListAsync();
    }

    public async Task<Book> Create(Book entity)
    {
        _libraryContext.Books.Add(entity);
        await _libraryContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Book> Update(Book entity)
    {
        _libraryContext.Entry(entity).State = EntityState.Modified;
        _libraryContext.Update(entity);
        await _libraryContext.SaveChangesAsync();
            
        return entity;
    }

    public async Task Delete(long id)
    {
        var entity = _libraryContext.Books.Find(id);
        if (_libraryContext.Entry(entity).State == EntityState.Detached)
        {
            _libraryContext.Books.Attach(entity);
        }
        _libraryContext.Books.Remove(entity);
    }

    public async Task<Book> GetById(long id)
    {
        return await _libraryContext.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    }
}