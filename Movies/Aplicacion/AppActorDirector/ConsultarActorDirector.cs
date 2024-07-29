using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppActorDirector;

public class ConsultarActorDirector
{
    public class ParameterosConsultarActorDirector : IRequest<List<ActorDirector>>
    {
        public int Opcion { get; set; }
        public string Criterio { get; set; }
    }

    public class EjecutaValidacion : AbstractValidator<ParameterosConsultarActorDirector>
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

    public class Manejador : IRequestHandler<ParameterosConsultarActorDirector, List<ActorDirector>>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<List<ActorDirector>> Handle(ParameterosConsultarActorDirector request,
            CancellationToken cancellationToken)
        {
            if (request.Opcion == 2)
                return await _context.ActorDirector.Where(x => x.Nombre.ToLower().Contains(request.Criterio.ToLower()))
                    .ToListAsync();
            if (request.Opcion == 3)
                return await _context.ActorDirector.Where(x =>
                        x.Nombre.ToLower().Contains(request.Criterio.ToLower()) && x.EsActor == true)
                    .ToListAsync();
            if (request.Opcion == 4)
                return await _context.ActorDirector.Where(x =>
                        x.Nombre.ToLower().Contains(request.Criterio.ToLower()) && x.EsActor == true)
                    .ToListAsync();
            return await _context.ActorDirector
                .ToListAsync();
        }

        public Task Handle(EjecutaValidacion request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}