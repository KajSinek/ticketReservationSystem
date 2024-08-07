﻿using AppCore.Entities;
using AppCore.Handlers.Accounts;
using AppCore.Models;
using Helpers.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BusReservationSystem.Controllers;

public partial class AccountsController
{
    [HttpPut]
    [Route("api/accounts/{account_id}/balance")]
    [SwaggerOperation("AddAccountBalance")]
    [SwaggerResponse(200, "Account balance added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<AccountBalance>))]
    public async Task<IActionResult> GetAccountBalanceAsync([Required, FromRoute(Name = "account_id")] Guid account_id,
                                                            [FromBody, Bind] AddAccountBalanceModel model,
                                                            CancellationToken ct)
    {
        var result = await mediator.Send(new AccountBalanceHandlerCommand { AccountId = account_id, Value = model.Value}, ct);

        if (result.IsFailure || result.Entity is null) return StatusCode((int)result.StatusCode, result);

        return Ok(result);
    }
}
