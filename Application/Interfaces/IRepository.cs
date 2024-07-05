//using Microsoft.AspNetCore.Mvc;
namespace Application.Interfaces;

public interface IRepository<T> where T : class
{
    public Task<IEnumerable<T>> Get();
    public Task<T> Create(T entity);
    public Task Update(int id, T entity);
    public Task Delete(int id);
}