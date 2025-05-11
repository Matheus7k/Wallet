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
    [ProducesResponseType(typeof(PostAuthenticateCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<PostAuthenticateCommandResponse> PostAuthenticateAsync([FromBody] PostAuthenticateCommand command) =>
        await mediator.Send(command);
}