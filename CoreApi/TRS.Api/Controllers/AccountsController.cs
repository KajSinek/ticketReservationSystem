using System.ComponentModel.DataAnnotations;
using Helpers.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;
using TRS.CoreApi.Mapping;
using TRS.CoreApi.Models;

namespace BusReservationSystem.Controllers;

[ApiController]
public partial class AccountsController(IMediator mediator) : ControllerBase
{
    public const string GetAccountRouteName = "GetAccount";

    [HttpPost]
    [Route("api/accounts")]
    [SwaggerOperation("AddAccount")]
    [SwaggerResponse(201, "Account added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(409, "Account Conflict")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityResponse<Account>))]
    public async Task<IActionResult> CreateAccountAsync(
        [FromBody, Bind] CreateAccountModel model,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new CreateAccountHandlerCommand
            {
                Username = model.Username,
                Email = model.Email,
                Address = model.Address,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            },
            ct
        );

        if (result.IsFailure || result.Entity is not Account account)
            return StatusCode((int)result.StatusCode, result);

        return CreatedAtRoute(
            GetAccountRouteName,
            routeValues: new { account_id = result.Entity.Id },
            value: account.ToAccountApiDto()
        );
    }

    [HttpGet]
    [Route("api/accounts")]
    [SwaggerOperation("GetAccounts")]
    [SwaggerResponse(200, "Accounts getted successfully")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntitiesResponse<Account>))]
    public async Task<IActionResult> GetAccountsAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAccountsHandlerQuery { }, ct);
        if (result.IsFailure || result.Entities is not List<Account> accounts)
            return StatusCode((int)result.StatusCode, result);

        return Ok(accounts.ToCollectionDto(item => item.ToAccountApiDto()));
    }

    [HttpGet]
    [Route("api/accounts/{account_id}", Name = GetAccountRouteName)]
    [SwaggerOperation("GetAccount")]
    [SwaggerResponse(200, "Account getted successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<Account>))]
    public async Task<IActionResult> GetAccountAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(new GetAccountHandlerQuery { AccountId = account_id }, ct);

        if (result.IsFailure || result.Entity is not Account account)
            return StatusCode((int)result.StatusCode, result);

        return Ok(account.ToAccountApiDto());
    }

    [HttpDelete]
    [Route("api/accounts/{account_id}")]
    [SwaggerOperation("DeleteAccount")]
    [SwaggerResponse(204, "Account deleted successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAccountAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new DeleteAccountHandlerCommand { AccountId = account_id },
            ct
        );

        if (result.IsFailure)
            return StatusCode((int)result.StatusCode, result);

        return NoContent();
    }

    [HttpPut]
    [Route("api/accounts/{account_id}")]
    [SwaggerOperation("DeleteAccount")]
    [SwaggerResponse(200, "Account updated successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<Account>))]
    public async Task<IActionResult> UpdateAccountAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        [FromBody, Bind] UpdateAccountModel model,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new UpdateAccountHandlerCommand
            {
                AccountId = account_id,
                Address = model.Address,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Username = model.Username
            },
            ct
        );

        if (result.IsFailure || result.Entity is not Account account)
            return StatusCode((int)result.StatusCode, result);

        return Ok(account.ToAccountApiDto());
    }
}
