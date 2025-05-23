using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Commands.v1.Users.AddUser;

namespace Wallet.Api.Controllers.v1;

[Route("api/v1/users")]
public class UserController(IMediator mediator) : Controller
{
    [HttpPost]
    [ActionName("CreateUser")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddUserAsync([FromBody] AddUserCommand command)
    {
        await mediator.Send(command);
        
        return Created();
    }
}