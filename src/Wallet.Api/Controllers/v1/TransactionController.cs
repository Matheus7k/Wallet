using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Commands.v1.Transactions.v1.AddDeposit;

namespace Wallet.Api.Controllers.v1;

[Route("api/v1/transactions")]
public class TransactionController(IMediator mediator) : Controller
{
    [HttpPost]
    [ActionName("CreateDeposit")]
    [Authorize]
    [Route("deposit")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddDepositAsync([FromBody] AddDepositCommand command)
    {
        await mediator.Send(command.SetEmail(User.Identity!.Name!));
        
        return Created();
    }
}