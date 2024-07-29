using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppPelicula;

public class ConsultarListadoPeliculas
{
    public class ParametrosConsultaListadoPeliculas : IRequest<List<Pelicula>>
    {
        public int Opcion { get; set; }
        public string Criterio { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
    }


    public class EjecutaValidacion : AbstractValidator<ParametrosConsultaListadoPeliculas>
    {
        public EjecutaValidacion()
        {
            RuleFor(x => x.Opcion).NotEmpty();
            RuleFor(x => x.Opcion).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Opcion).LessThanOrEqualTo(4);
            RuleFor(x => new { x.Criterio, x.Opcion }).Must(x => ValidarQueEnvieCriterio(x.Opcion, x.Criterio))
                .WithMessage("Para la opcion debe enviar el criterio");
            ;
        }

        private bool ValidarQueEnvieCriterio(int opcion, string criterio)
        {
            if (opcion == 2)
            {
                if (criterio.Length == 0) return false;

                return true;
            }

            return true;
        }
    }


    public class Manejador : IRequestHandler<ParametrosConsultaListadoPeliculas, List<Pelicula>>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<List<Pelicula>> Handle(ParametrosConsultaListadoPeliculas request,
            CancellationToken cancellationToken)
        {
            if (request.Opcion == 1)
            {
                var peliculas = await _context.Pelicula
                    .Include(x => x.Actorlnk)
                    .ThenInclude(x => x.ActorDirector)
                    .Include(x => x.DirectorLnk)
                    .ThenInclude(x => x.ActorDirector).ToListAsync();
                return peliculas;
            }
            else
            {
                var peliculas = await _context.Pelicula
                    .Include(x => x.Actorlnk)
                    .ThenInclude(x => x.ActorDirector)
                    .Include(x => x.DirectorLnk)
                    .ThenInclude(x => x.ActorDirector).ToListAsync();
                return peliculas;
            }
        }
    }
}