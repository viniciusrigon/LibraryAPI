using Domain.Entities;

namespace Application.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> Get();
    Task<Book> Insert(Book book);
    Task<Book> Update(Book book);
    Task Delete(long id);

}