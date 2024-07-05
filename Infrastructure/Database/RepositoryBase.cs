using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces;

namespace Infrastructure;

public abstract class RepositoryBase<T> : IRepository<T> where T:class
{
    protected readonly DbContext _context;
    protected DbSet<T> dbSet;
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryBase(IUnitOfWork unitOfwork)
    {
        _unitOfWork = unitOfwork;
        dbSet = _unitOfWork.Context.Set<T>();
    }
    
    public async Task<IEnumerable<T>> Get()
    {
        var data = await dbSet.ToListAsync();
        return data;
    }

    public async Task<T> Create(T entity)
    {
        dbSet.Add(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task Update(int id, T entity)
    {
        var existingOrder = await dbSet.FindAsync(id);
        
        _unitOfWork.Context.Entry(existingOrder).CurrentValues.SetValues(entity);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    public async Task Delete(int id)
    {
        var data = await dbSet.FindAsync(id);
        if (data == null)
        {
            
        }

        dbSet.Remove(data);
        await _unitOfWork.SaveChangesAsync();
        
    }
}