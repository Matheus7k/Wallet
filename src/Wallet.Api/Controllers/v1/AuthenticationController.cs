using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;

namespace Wallet.Api.Controllers.v1;

[Route("api/v1")]
public class AuthenticationController(IMediator mediator) : Controller
{
    [Route("authenticate")]
    [HttpPost]
    [ActionName("Authenticate")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAuthenticateAsync([FromBody] PostAuthenticateCommand command) =>
        Ok(await mediator.Send(command));
}