using Newtonsoft.Json;
using System.Net;

public interface IResponse
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
    List<string> Errors { get; }
    HttpStatusCode StatusCode { get; }
    bool IsFailure { get; }

    Response AddError(string error);

    Response SetStatusCode(HttpStatusCode statusCode);
}
