using Atlanta.Domain.Entity;

namespace Atlanta.DAL.Interfaces;

public interface IBaseRepository<T>
{
    Task<IEnumerable<T>> SelectAsync();
    Task<bool> Add(T entity);
    Task<bool> Delete(T entity);
    Task<T> GetById(int id);
    Task<bool> Update(T entity);
}