using System.ComponentModel.DataAnnotations;
using Helpers.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.AccountBalances;
using TRS.CoreApi.Mapping;
using TRS.CoreApi.Models;

namespace BusReservationSystem.Controllers;

public partial class AccountsController
{
    [HttpGet]
    [Route("api/accounts/{account_id}/balance")]
    [SwaggerOperation("GetAccountBalance")]
    [SwaggerResponse(200, "OK")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<AccountBalance>))]
    public async Task<IActionResult> GetAccountBalanceAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new GetAccountBalanceQuery { AccountId = account_id},
            ct
        );

        if (result.IsFailure || result.Entity is not AccountBalance accountBalance)
            return StatusCode((int)result.StatusCode, result);

        return Ok(accountBalance.ToAccountBalanceApiDto());
    }

    [HttpPut]
    [Route("api/accounts/{account_id}/balance")]
    [SwaggerOperation("UpdateAccountBalance")]
    [SwaggerResponse(200, "AccountBalance updated")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<AccountBalance>))]
    public async Task<IActionResult> UpdateAccountBalanceAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        [FromBody, Bind] UpdateAccountBalanceModel model,
        CancellationToken ct
    )
    {
        var result = await mediator.Send(
            new UpdateAccountBalanceCommand
            {
                AccountId = account_id,
                Value = model.Value,
                Type = model.Type
            },
            ct
        );

        if (result.IsFailure || result.Entity is not AccountBalance accountBalance)
            return StatusCode((int)result.StatusCode, result);

        return Ok(accountBalance.ToAccountBalanceApiDto());
    }
}
