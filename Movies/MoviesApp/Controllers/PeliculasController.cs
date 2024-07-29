using Aplicacion.AppPelicula;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace MoviesApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeliculasController : MiControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<List<Pelicula>>> GetList(
        ConsultarListadoPeliculas.ParametrosConsultaListadoPeliculas data)
    {
        return await Mediator.Send(data);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Pelicula>> Detalle(int id)
    {
        return await Mediator.Send(new ConsultaPeliculaID.ParametroConsultaPeliculaID { Id = id });
    }


    [HttpPost("ActorDirector")]
    public async Task<ActionResult<List<ActorDirector>>> listaActoresDirectores(
        ConsultaPeliculaActorDirector.ParameterosPeliculaActorDirector data)
    {
        return await Mediator.Send(data);
    }

    [HttpPost("Crear")]
    public async Task<ActionResult<ResponseOperations>> Crear(CrearPelicula.CreacionPeliculaParametros data)
    {
        return await Mediator.Send(data);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseOperations>> Actualizar(int id,
        ActualizarPelicula.ActualizarPeliculaParametros data)
    {
        data.PeliculaID = id;
        return await Mediator.Send(data);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseOperations>> Eliminar(int id)
    {
        return await Mediator.Send(new EliminarPelicula.ParametrosEliminarPelicula { Id = id });
    }
}