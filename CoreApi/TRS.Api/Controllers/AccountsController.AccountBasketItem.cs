using System.ComponentModel.DataAnnotations;
using Helpers.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.AccountBasketItems;
using TRS.CoreApi.Handlers.Accounts;
using TRS.CoreApi.Mapping;
using TRS.CoreApi.Models;

namespace BusReservationSystem.Controllers;

public partial class AccountsController
{
    [HttpPost]
    [Route("api/accounts/{account_id}/basket")]
    [SwaggerOperation("AddAccountBasketItem")]
    [SwaggerResponse(201, "Account basket item added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityResponse<AccountBasketTicket>))]
    public async Task<IActionResult> CreateAccountBasketTicketAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        [FromBody, Bind] CreateAccountBasketItemModel model,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new CreateAccountBasketItemCommand
            {
                AccountId = account_id,
                TicketId = model.TicketId,
                Amount = model.Amount
            },
            ct
        );

        if (result.IsFailure || result.Entity is null)
            return StatusCode((int)result.StatusCode, result);

        return CreatedAtRoute(
            routeValues: new { account_id },
            value: result
        );
    }

    [HttpDelete]
    [Route("api/accounts/{account_id}/basket")]
    [SwaggerOperation("AddAccountBasketItem")]
    [SwaggerResponse(201, "Account basket item added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityResponse<AccountBasketTicket>))]
    public async Task<IActionResult> DeleteAccountBasketTicketAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        [FromBody, Bind] DeleteAccountBasketTicketModel model,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new DeleteAccountBasketItemCommand
            {
                AccountId = account_id,
                TicketId = model.TicketId
            },
            ct
        );

        if (result.IsFailure)
            return StatusCode((int)result.StatusCode, result);

        return NoContent();
    }

    [HttpPut]
    [Route("api/accounts/{account_id}/basket")]
    [SwaggerOperation("AddAccountBasketItem")]
    [SwaggerResponse(201, "Account basket item added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityResponse<AccountBasketTicket>))]
    public async Task<IActionResult> UpdateAccountBasketTicketAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        [FromBody, Bind] UpdateAccountBasketTicketModel model,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new UpdateAccountBasketItemCommand
            {
                AccountId = account_id,
                TicketId = model.TicketId,
                Amount = model.Amount
            },
            ct
        );

        if (result.IsFailure || result.Entity is not AccountBasketTicket accountBasketTicket)
            return StatusCode((int)result.StatusCode, result);

        return Ok(accountBasketTicket.ToAccountBasketTicketApiDto());
    }

    [HttpGet]
    [Route("api/accounts/{account_id}/basket")]
    [SwaggerOperation("GetBasket")]
    [SwaggerResponse(200, "Accounts getted successfully")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntitiesResponse<AccountBasketTicket>))]
    public async Task<IActionResult> GetAccountsAsync([Required, FromRoute(Name = "account_id")] Guid account_id,
                                                      CancellationToken ct)
    {
        var result = await mediator.Send(new ListAccountBasketItemCommand { AccountId = account_id }, ct);
        if (result.IsFailure || result.Entities is not List<AccountBasketTicket> accountBasketTickets)
            return StatusCode((int)result.StatusCode, result);

        return Ok(accountBasketTickets.ToCollectionDto(item => item.ToAccountBasketTicketApiDto()));
    }
}
