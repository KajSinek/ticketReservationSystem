namespace Helpers.Responses;

public class EntityResponse<T> : Response
{
    public T? Entity { get; set; }

    public void SetEntity(T entity)
    {
        Entity = entity;
    }
}