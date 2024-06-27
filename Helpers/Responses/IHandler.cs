namespace Helpers.Responses;

public interface IHandler<in TRequest, TResponse> : IHandler
where TResponse : IResponse, new()
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken ct = default);
}

public interface IHandler
{ }