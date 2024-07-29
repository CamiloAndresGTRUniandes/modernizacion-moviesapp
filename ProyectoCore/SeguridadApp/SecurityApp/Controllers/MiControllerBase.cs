using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SecurityApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MiControllerBase : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
}