using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    DbContext Context { get; }
    public Task SaveChangesAsync();
}