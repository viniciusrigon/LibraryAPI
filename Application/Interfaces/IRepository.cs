//using Microsoft.AspNetCore.Mvc;
namespace Application.Interfaces;

public interface IRepository<T> where T : class
{
    public Task<IEnumerable<T>> Get();
    public Task<T> Create(T entity);
    public Task<T> Update(T entity);
    public Task Delete(long id);
}