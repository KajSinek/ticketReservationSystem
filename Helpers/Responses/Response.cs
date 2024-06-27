using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;

public class EmptyListConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<string>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var list = (List<string>)value;
        if (list != null && list.Count > 0)
        {
            serializer.Serialize(writer, list);
        }
        else
        {
            writer.WriteNull();
        }
    }
}

public class Response : IResponse
{
    [JsonConverter(typeof(EmptyListConverter))]
    public List<string> Errors { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsFailure => Errors.Count > 0;

    public Response()
    {
        Errors = new List<string>();
    }

    public virtual Response AddError(string error)
    {
        Errors.Add(error);
        return this;
    }

    public virtual Response SetStatusCode(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        return this;
    }
}