using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryContext _context;
    private bool _disposed = false;
    
    public UnitOfWork(LibraryContext context)
    {
        _context = context;
    }
    
    public DbContext Context => _context;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}