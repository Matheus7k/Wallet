using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Commands.v1.Transactions.v1.AddDeposit;
using Wallet.Application.Commands.v1.Transactions.v1.AddTransfer;
using Wallet.Application.Queries.v1.Transactions.GetBalance;
using Wallet.Application.Queries.v1.Transactions.GetTransactions;

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
    
    [HttpPost]
    [ActionName("CreateTransfer")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("transfer")]
    public async Task<IActionResult> AddTransferAsync([FromBody] AddTransferCommand command)
    {
        await mediator.Send(command.SetFromEmail(User.Identity!.Name!));
        
        return Created();
    }

    [HttpGet]
    [ActionName("GetTransactions")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetTransactionsQueryResponse> GetTransactionsAsync([FromQuery] GetTransactionsQuery query) =>
        await mediator.Send(query.SetEmail(User.Identity!.Name!));
    
    [HttpGet]
    [ActionName("GetBalance")]
    [Authorize]
    [Route("balance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetBalanceQueryResponse> GetBalanceAsync() =>
        await mediator.Send(new GetBalanceQuery(User.Identity!.Name!));
}