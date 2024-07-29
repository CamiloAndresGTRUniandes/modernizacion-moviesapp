using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppPeliculaAlquiler;

public class CancelarAlquilerPelicula
{
    public class CancelarAlquilerPeliculaParametros : IRequest<ResponseOperations>
    {
        public int Id { get; set; }
    }

    public class Validations : AbstractValidator<CancelarAlquilerPeliculaParametros>
    {
        public Validations()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Manejador : IRequestHandler<CancelarAlquilerPeliculaParametros, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<ResponseOperations> Handle(CancelarAlquilerPeliculaParametros request,
            CancellationToken cancellationToken)
        {
            var alquiler = await _context.PeliculaAlquiler.Where(p => p.PeliculaAlquilerID == request.Id)
                .FirstOrDefaultAsync();
            if (alquiler == null)
                return new ResponseOperations { Ok = false, Message = "Ups esta reserva no existe", Id = 0 };

            alquiler.EstadoAquilerID = 3;
            alquiler.FechaCancelacion = DateTime.Now;

            var pelicula = await _context.Pelicula.Where(p => p.PeliculaID == alquiler.PeliculaID)
                .FirstOrDefaultAsync();

            if (pelicula == null)
                return new ResponseOperations { Ok = false, Message = "esta pelicula no existe :(", Id = 0 };

            pelicula.Disponible = true;


            var valor = await _context.SaveChangesAsync();

            if (valor > 0)
                return new ResponseOperations
                    { Ok = true, Message = "Tu la cancelacion ya fue generada", Id = alquiler.PeliculaAlquilerID };
            return new ResponseOperations { Ok = false, Message = "Ups esta cancelacion no se puede guardar", Id = 0 };
        }
    }
}