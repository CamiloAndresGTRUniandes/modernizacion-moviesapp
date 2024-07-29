using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityApp.Controllers;

namespace SecurityApp.Controllers;

[AllowAnonymous]
public class UsuarioController : MiControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioData>> Login(Login.EjecutaLogin parametros)
    {
        return await Mediator.Send(parametros);
    }


    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioData>> Registrar(Registrar.EjecutaRegistrar parametros)
    {
        return await Mediator.Send(parametros);
    }

    [HttpGet]
    public async Task<ActionResult<UsuarioData>> DevolverUsuario()
    {
        return await Mediator.Send(new UsuarioActual.Ejecutar());
    }

    [HttpPut]
    public async Task<ActionResult<UsuarioData>> Actualizar(UsuarioActualizar.EjecutaUsuarioActualizar parametros)
    {
        return await Mediator.Send(parametros);
    }
}