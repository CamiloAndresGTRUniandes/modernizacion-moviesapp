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

public class CrearAlquilerPelicula
{
    public class CrearAlquilerPeliculaParametros : IRequest<ResponseOperations>
    {
        public int PeliculaID { get; set; }

        public string UserName { get; set; }
    }

    public class Validations : AbstractValidator<CrearAlquilerPeliculaParametros>
    {
        public Validations()
        {
            RuleFor(x => x.PeliculaID).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }

    public class Manejador : IRequestHandler<CrearAlquilerPeliculaParametros, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<ResponseOperations> Handle(CrearAlquilerPeliculaParametros request,
            CancellationToken cancellationToken)
        {
            var pelicula = await _context.Pelicula.Where(p => p.PeliculaID == request.PeliculaID).FirstOrDefaultAsync();

            if (pelicula == null)
                return new ResponseOperations { Ok = false, Message = "esta pelicula no existe :(", Id = 0 };

            var peliculasAlquiladas = await _context.PeliculaAlquiler
                .Where(p => p.PeliculaID == request.PeliculaID && p.EstadoAquilerID == 1).CountAsync();


            if (!pelicula.Disponible)
                return new ResponseOperations { Ok = false, Message = "Esta pelicula no esta disponible :(", Id = 0 };

            if (pelicula.CantidadPeliculas >= peliculasAlquiladas + 1)
            {
                var PeliculaAlquiler = new PeliculaAlquiler
                {
                    PeliculaID = request.PeliculaID,
                    UserName = request.UserName,
                    FechaAlquiler = DateTime.Now,
                    FechaDebeEntregar = DateTime.Now.Date.AddDays(6),
                    EstadoAquilerID = 1
                };

                _context.Add(PeliculaAlquiler);

                if (pelicula.CantidadPeliculas == peliculasAlquiladas + 1) pelicula.Disponible = false;


                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return new ResponseOperations
                        { Ok = true, Message = "Tu reserva ya fue generada", Id = PeliculaAlquiler.PeliculaID };
                return new ResponseOperations { Ok = false, Message = "Tu reserva no fue generada", Id = 0 };
            }

            pelicula.Disponible = false;

            await _context.SaveChangesAsync();
            return new ResponseOperations
                { Ok = false, Message = "Tu reserva no fue generada ya no tenemos mas pelis", Id = 0 };
        }
    }
}