using Newtonsoft.Json;

namespace Helpers.Responses;

public class EntitiesResponse<T> : Response
{
    public List<T>? Entities { get; set; }

    public void SetEntity(List<T> entity)
    {
        Entities = entity;
    }
}