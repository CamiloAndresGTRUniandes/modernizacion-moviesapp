using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppPelicula;

public class ConsultaPeliculaID
{
    public class ParametroConsultaPeliculaID : IRequest<Pelicula>
    {
        public int Id { get; set; }
    }


    public class EjecutaValidacion : AbstractValidator<ParametroConsultaPeliculaID>
    {
        public EjecutaValidacion()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }


    public class Manejador : IRequestHandler<ParametroConsultaPeliculaID, Pelicula>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<Pelicula> Handle(ParametroConsultaPeliculaID request, CancellationToken cancellationToken)
        {
            var pelicula = await _context.Pelicula.Where(x => x.PeliculaID == request.Id)
                .Include(x => x.Actorlnk)
                .ThenInclude(x => x.ActorDirector)
                .Include(x => x.DirectorLnk)
                .ThenInclude(x => x.ActorDirector)
                .FirstOrDefaultAsync();
            return pelicula;
        }
    }
}