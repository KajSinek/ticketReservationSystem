﻿using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using Helpers.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;
using TRS.CoreApi.Handlers.Tickets;

namespace TRS.CoreApi.Handlers.AccountBasketItems;

public class  CreateAccountBasketItemCommand : IRequest<EntityResponse<AccountBasketTicket>>
{
    public required Guid AccountId { get; set; }
    public required Guid TicketId { get; set; }
    public required int Amount { get; set; }
}

public class CreateAccountBasketItemHandler(IBaseDbRequests baseRequest,
                                            IMediator mediator,
                                             ILogger<CreateAccountBasketItemHandler> logger)
    : IRequestHandler<CreateAccountBasketItemCommand, EntityResponse<AccountBasketTicket>>
{
    public async Task<EntityResponse<AccountBasketTicket>> Handle(
        CreateAccountBasketItemCommand request,
        CancellationToken ct
    )
    {
        var accountResponse = await mediator.Send(new GetAccountHandlerQuery { AccountId = request.AccountId }, ct);
        if (accountResponse.Entity is null || accountResponse.Entity is not Account account || account.AccountBasket is not AccountBasket accountBasket)
        {
            logger.LogError("Account does not exist");
            return new EntityResponse<AccountBasketTicket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Account), request.AccountId) }
            };
        }

        var ticketResponse = await mediator.Send(new GetTicketHandlerQuery { TicketId = request.TicketId }, ct);
        if (ticketResponse.Entity is null || ticketResponse.Entity is not Ticket ticket)
        {
            logger.LogError("Ticket does not exist");
            return new EntityResponse<AccountBasketTicket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Ticket), request.TicketId) }
            };
        }

        var entity = new AccountBasketTicket
        {
            AccountBasketId = accountBasket.Id,
            TicketId = request.TicketId,
            Amount = request.Amount,
            CreatedOn = DateHelper.DateTimeUtcNow
        };

        var accountBasketTicketResponse = await baseRequest.CreateAsync(entity);
        if (accountBasketTicketResponse.Entity is null || accountBasketTicketResponse.Entity is not AccountBasketTicket accountBasketTicket)
        {
            logger.LogError("Failed to create Account Basket Item");
            return accountBasketTicketResponse;
        }
        return accountBasketTicketResponse;
    }
}
