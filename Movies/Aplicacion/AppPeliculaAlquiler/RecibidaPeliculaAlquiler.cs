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

public class RecibidaPeliculaAlquiler
{
    public class RecibidaPeliculaAlquilerParametros : IRequest<ResponseOperations>
    {
        public int Id { get; set; }
    }

    public class Validations : AbstractValidator<RecibidaPeliculaAlquilerParametros>
    {
        public Validations()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Manejador : IRequestHandler<RecibidaPeliculaAlquilerParametros, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<ResponseOperations> Handle(RecibidaPeliculaAlquilerParametros request,
            CancellationToken cancellationToken)
        {
            var alquiler = await _context.PeliculaAlquiler.Where(p => p.PeliculaAlquilerID == request.Id)
                .FirstOrDefaultAsync();
            if (alquiler == null)
                return new ResponseOperations { Ok = false, Message = "Ups esta reserva no existe", Id = 0 };
            alquiler.EstadoAquilerID = 2;
            alquiler.FechaEntrega = DateTime.Now;

            var pelicula = await _context.Pelicula.Where(p => p.PeliculaID == alquiler.PeliculaID)
                .FirstOrDefaultAsync();
            pelicula.Disponible = true;
            var valor = await _context.SaveChangesAsync();


            if (valor > 0)
                return new ResponseOperations
                    { Ok = true, Message = "La pelicula ha sido recibida", Id = alquiler.PeliculaAlquilerID };
            return new ResponseOperations
                { Ok = false, Message = "La pelicula ha sido recibida no se puede guardar", Id = 0 };
        }
    }
}