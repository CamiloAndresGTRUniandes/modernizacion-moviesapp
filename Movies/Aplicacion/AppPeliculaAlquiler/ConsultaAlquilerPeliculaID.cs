using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppPeliculaAlquiler;

public class ConsultaAlquilerPeliculaID
{
    public class ParametroConsultaAlquilerPeliculaID : IRequest<PeliculaAlquiler>
    {
        public int Id { get; set; }
        public int Opcion { get; set; }
    }


    public class EjecutaValidacion : AbstractValidator<ParametroConsultaAlquilerPeliculaID>
    {
        public EjecutaValidacion()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }


    public class Manejador : IRequestHandler<ParametroConsultaAlquilerPeliculaID, PeliculaAlquiler>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<PeliculaAlquiler> Handle(ParametroConsultaAlquilerPeliculaID request,
            CancellationToken cancellationToken)
        {
            var PeliculaAlquiler = await _context.PeliculaAlquiler.Where(x => x.PeliculaAlquilerID == request.Id)
                .Include(x => x.Pelicula)
                .FirstOrDefaultAsync();
            return PeliculaAlquiler;
        }
    }
}