using System.ComponentModel.DataAnnotations;
using Helpers.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Models;

namespace TRS.Api.Controllers;

public partial class AccountsController
{
    [HttpPost]
    [Route("api/accounts/{account_id}/basket")]
    [SwaggerOperation("AddAccountBasketItem")]
    [SwaggerResponse(201, "Account basket item added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Account not found")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityResponse<AccountBasket>))]
    public async Task<IActionResult> AddAccountBasketItemAsync(
        [Required, FromRoute(Name = "account_id")] Guid account_id,
        [FromBody, Bind] CreateAccountBasketItemModel model,
        CancellationToken ct
    )
    {
        throw new NotImplementedException();
        /*var result = await mediator.Send(   
            new CreateAccountBasketItemModel
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
            GetAccountBasketItemRouteName,
            routeValues: new { account_id = account_id, basket_item_id = result.Entity.BasketItemId },
            value: result
        );*/
    }
}
