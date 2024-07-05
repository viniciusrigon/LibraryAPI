using Domain.Entities;

namespace Application.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<Book> GetById(int id);

}