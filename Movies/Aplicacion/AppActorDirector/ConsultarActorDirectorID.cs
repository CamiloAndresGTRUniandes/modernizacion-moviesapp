using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.AppActorDirector;

public class ConsultarActorDirectorID
{
    public class ParameterosConsultarAutorDirector : IRequest<ActorDirector>
    {
        public int Id { get; set; }
    }


    public class Validations : AbstractValidator<ParameterosConsultarAutorDirector>
    {
        public Validations()
        {
            RuleFor(x => x.Id).GreaterThan(-1).WithMessage("The id must be greater than or equal to zero");
        }

        private object RuleFor(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }

    public class Manejador : IRequestHandler<ParameterosConsultarAutorDirector, ActorDirector>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<ActorDirector> Handle(ParameterosConsultarAutorDirector request,
            CancellationToken cancellationToken)
        {
            return await _context.ActorDirector.Where(p => p.ActorDirectorID == request.Id).FirstOrDefaultAsync();
        }
    }
}