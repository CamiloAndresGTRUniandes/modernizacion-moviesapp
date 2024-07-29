using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppPelicula;

public class ConsultaPeliculaActorDirector
{
    public class ParameterosPeliculaActorDirector : IRequest<List<ActorDirector>>
    {
        public int id { get; set; }

        public int opcion { get; set; }
    }


    public class Validations : AbstractValidator<ParameterosPeliculaActorDirector>
    {
        public Validations()
        {
            RuleFor(x => x.id).GreaterThan(-1);
        }
    }


    public class Manejador : IRequestHandler<ParameterosPeliculaActorDirector, List<ActorDirector>>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<List<ActorDirector>> Handle(ParameterosPeliculaActorDirector request,
            CancellationToken cancellationToken)
        {
            if (request.id == 0)
            {
                if (request.opcion == 1)
                    return await _context.ActorDirector.Where(p => p.EsActor == true).ToListAsync();
                return await _context.ActorDirector.Where(p => p.EsDirector == true).ToListAsync();
            }

            if (request.opcion == 1)
            {
                var resultadoPP = await _context.PeliculaActor.Where(p => p.PeliculaID == request.id).ToListAsync();
                var resultadosP = await _context.ActorDirector.Where(p => p.EsActor == true).ToListAsync();

                var result = (from p in resultadosP
                            join pp in resultadoPP on p.ActorDirectorID equals pp.ActorDirectorID
                                into ps
                            from pp in ps.DefaultIfEmpty()
                            select new ActorDirector
                            {
                                urlFoto = p.urlFoto, ActorDirectorID = p.ActorDirectorID, Nombre = p.Nombre,
                                Seleccionado = pp == null ? false : true
                            }
                        ).ToList()
                    ;

                return result;
            }
            else
            {
                var resultadoPP = await _context.PeliculaDirector.Where(p => p.PeliculaID == request.id).ToListAsync();
                var resultadosP = await _context.ActorDirector.Where(p => p.EsDirector == true).ToListAsync();

                var result = (from p in resultadosP
                            join pp in resultadoPP on p.ActorDirectorID equals pp.ActorDirectorID
                                into ps
                            from pp in ps.DefaultIfEmpty()
                            select new ActorDirector
                            {
                                urlFoto = p.urlFoto, ActorDirectorID = p.ActorDirectorID, Nombre = p.Nombre,
                                Seleccionado = pp == null ? false : true
                            }
                        ).ToList()
                    ;

                return result;
            }
        }
    }
}