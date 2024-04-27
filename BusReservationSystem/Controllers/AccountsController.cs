using AppCore.Handlers.Accounts;
using AppCore.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BusReservationSystem.Controllers;

[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("api/accounts")]
    [SwaggerOperation("AddInvestmentAccount")]
    [SwaggerResponse(200, "Investment Account added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    public async Task<IActionResult> CreateAccountAsync([FromBody, Bind] CreateAccountModel model, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateAccountHandlerCommand { Username = model.Username, Email = model.Email });
        return Ok(result);
    }
}
