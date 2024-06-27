using MediatR;

namespace Helpers.Responses;

public interface IHandlerContext
{
    IMediator Mediator { get; }
}