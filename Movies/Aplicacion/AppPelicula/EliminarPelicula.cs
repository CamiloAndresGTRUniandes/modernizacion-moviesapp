using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.AppPelicula;

public class EliminarPelicula
{
    public class ParametrosEliminarPelicula : IRequest<ResponseOperations>
    {
        public int Id { get; set; }
    }

    public class Manejador : IRequestHandler<ParametrosEliminarPelicula, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<ResponseOperations> Handle(ParametrosEliminarPelicula request,
            CancellationToken cancellationToken)
        {
            var ActorBD = _context.PeliculaActor.Where(x => x.PeliculaID == request.Id);
            foreach (var ActorEliminar in ActorBD) _context.PeliculaActor.Remove(ActorEliminar);


            var DirectorBD = _context.PeliculaDirector.Where(x => x.PeliculaID == request.Id);
            foreach (var DirectorEliminar in DirectorBD) _context.PeliculaDirector.Remove(DirectorEliminar);


            var pelicula = await _context.Pelicula.FindAsync(request.Id);
            if (pelicula == null)
                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el pelicula" });
            _context.Remove(pelicula);


            var resultado = await _context.SaveChangesAsync();


            if (resultado > 0)
                return new ResponseOperations
                    { Ok = true, Message = "La pelicula fue eliminada", Id = pelicula.PeliculaID };
            return new ResponseOperations { Ok = false, Message = "La pelicula no fue eliminada ", Id = 0 };

            throw new Exception("No se pudieron guardar los cambios");
        }
    }
}