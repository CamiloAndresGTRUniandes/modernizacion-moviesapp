using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.AppActorDirector;

public class EliminarActorDirector
{
    public class ParameterosEliminarActorDirector : IRequest<ResponseOperations>
    {
        public int Id { get; set; }
    }

    public class Manejador : IRequestHandler<ParameterosEliminarActorDirector, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }


        public async Task<ResponseOperations> Handle(ParameterosEliminarActorDirector request,
            CancellationToken cancellationToken)
        {
            var ActorDirector = await _context.ActorDirector.FindAsync(request.Id);
            if (ActorDirector == null)
                return new ResponseOperations { Ok = false, Message = "No se encontro el actor director ", Id = 0 };

            _context.ActorDirector.Remove(ActorDirector);

            var item = await _context.SaveChangesAsync();

            return new ResponseOperations { Ok = true, Message = "Se elimino el actor director ", Id = 0 };
        }
    }
}