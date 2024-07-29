using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppPeliculaAlquiler;

public class ConsultaAlquilerPelicula
{
    public class ParametroConsultaAlquilerPelicula : IRequest<List<PeliculaAlquiler>>
    {
        public int Id { get; set; }
        public int Opcion { get; set; }
        public string UserName { get; set; }
    }


    public class EjecutaValidacion : AbstractValidator<ParametroConsultaAlquilerPelicula>
    {
        public EjecutaValidacion()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Opcion).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Opcion).LessThanOrEqualTo(4);
        }
    }


    public class Manejador : IRequestHandler<ParametroConsultaAlquilerPelicula, List<PeliculaAlquiler>>
    {
        private readonly VideoBlockOnlineContext _context;
        private readonly UserManager<Usuario> _usuarioManager;

        public Manejador(VideoBlockOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            _context = context;
            _usuarioManager = usuarioManager;
        }

        public async Task<List<PeliculaAlquiler>> Handle(ParametroConsultaAlquilerPelicula request,
            CancellationToken cancellationToken)
        {
            //Por  un alquiler
            if (request.Opcion == 1)
            {
                var PeliculaAlquiler = await _context.PeliculaAlquiler.Where(x => x.EstadoAquilerID == 1)
                    .Include(x => x.EstadoAlquiler)
                    .Include(x => x.Pelicula)
                    .ToListAsync();
                var usuarios = await _usuarioManager.Users.ToListAsync();

                var lst = (from d in usuarios
                    join x in PeliculaAlquiler on d.Email equals x.UserName //into un
                    select new PeliculaAlquiler
                    {
                        UserName = x.UserName,
                        PeliculaID = x.PeliculaID,
                        FechaAlquiler = x.FechaAlquiler,
                        FechaEntrega = x.FechaEntrega,
                        FechaDebeEntregar = x.FechaDebeEntregar,
                        Pelicula = x.Pelicula,
                        EstadoAquilerID = x.EstadoAquilerID,
                        NombreUsuario = d.NombreCompleto,
                        PeliculaAlquilerID = x.PeliculaAlquilerID,
                        EstadoAlquiler = x.EstadoAlquiler
                    }).ToList();


                return lst;
            }

            //Por  una pelicula
            if (request.Opcion == 2)
            {
                var PeliculaAlquiler = await _context.PeliculaAlquiler.Where(x => x.PeliculaID == request.Id)
                    .Include(x => x.Pelicula)
                    .Include(x => x.EstadoAlquiler)
                    .ToListAsync();
                return PeliculaAlquiler;
            }

            //Por  una persona
            if (request.Opcion == 3)
            {
                var PeliculaAlquiler = await _context.PeliculaAlquiler.Where(x => x.UserName == request.UserName)
                    .Include(x => x.Pelicula)
                    .Include(x => x.EstadoAlquiler)
                    .ToListAsync();
                return PeliculaAlquiler;
            }

            if (request.Opcion == 4)
            {
                var PeliculaAlquiler = await _context.PeliculaAlquiler
                    .Include(x => x.EstadoAlquiler)
                    .Include(x => x.Pelicula)
                    .ToListAsync();
                var usuarios = await _usuarioManager.Users.ToListAsync();

                var lst = (from d in usuarios
                    join x in PeliculaAlquiler on d.Email equals x.UserName //into un
                    select new PeliculaAlquiler
                    {
                        UserName = x.UserName,
                        PeliculaID = x.PeliculaID,
                        FechaAlquiler = x.FechaAlquiler,
                        FechaEntrega = x.FechaEntrega,
                        FechaDebeEntregar = x.FechaDebeEntregar,
                        Pelicula = x.Pelicula,
                        EstadoAquilerID = x.EstadoAquilerID,
                        NombreUsuario = d.NombreCompleto,
                        PeliculaAlquilerID = x.PeliculaAlquilerID,
                        EstadoAlquiler = x.EstadoAlquiler,
                        FechaCancelacion = x.FechaCancelacion
                    }).ToList();


                return lst;
            }

            return new List<PeliculaAlquiler>();
        }
    }
}