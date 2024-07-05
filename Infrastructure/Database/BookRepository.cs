using Domain.Entities;
using Application.Interfaces;

namespace Infrastructure;

public class BookRepository : RepositoryBase<Book>
{
    public BookRepository(IUnitOfWork unitOfwork) : base(unitOfwork)
    {
    }
}