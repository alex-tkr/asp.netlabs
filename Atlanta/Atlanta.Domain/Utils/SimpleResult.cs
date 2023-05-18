
namespace Atlanta.Domain.Utils;

public class SimpleResult<T> : ISimpleResult<T> 
{
    public bool HasError { get; set; }
    public T? Entity { get; set; }

    public string Description { get; set; }

    public static SimpleResult<T> FromSuccess(T entity)
    {
        return new SimpleResult<T>() { Entity = entity, Description = "", HasError = false };
    }

    public static SimpleResult<T> FromError(string msg)
    {
        return new SimpleResult<T>() { Entity = default(T), Description = msg, HasError = true };
    }
}

public interface ISimpleResult<T>
{
    public bool HasError { get; set; }
    public T? Entity { get; set; }
}