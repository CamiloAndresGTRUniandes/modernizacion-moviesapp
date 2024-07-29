using Aplicacion.AppActorDirector;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace MoviesApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorDirectorController : MiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<List<ActorDirector>>> GetList(
        ConsultarActorDirector.ParameterosConsultarActorDirector data)
    {
        return await Mediator.Send(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDirector>> Detalle(int id)
    {
        return await Mediator.Send(new ConsultarActorDirectorID.ParameterosConsultarAutorDirector { Id = id });
    }

    [HttpPost("Creacion")]
    public async Task<ActionResult<ResponseOperations>> Crear(CrearActorDirector.CreateBookParameters data)
    {
        return await Mediator.Send(data);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseOperations>> Editar(int id,
        ActualizarActorDirector.ParametrosActorDirector data)
    {
        data.ActorDirectorID = id;
        return await Mediator.Send(data);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseOperations>> Eliminar(int id)
    {
        return await Mediator.Send(new EliminarActorDirector.ParameterosEliminarActorDirector { Id = id });
    }
}