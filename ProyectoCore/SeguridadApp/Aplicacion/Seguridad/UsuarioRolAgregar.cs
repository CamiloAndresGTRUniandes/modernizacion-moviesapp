using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad;

public class UsuarioRolAgregar
{
    public class EjecutarUsuarioRolAgregar : IRequest<Unit>
    {
        public string Username { get; set; }
        public string RolNombre { get; set; }
    }

    public class EjecutaValidador : AbstractValidator<EjecutarUsuarioRolAgregar>
    {
        public EjecutaValidador()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.RolNombre).NotEmpty();
        }
    }

    public class Manejador : IRequestHandler<EjecutarUsuarioRolAgregar, Unit>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Usuario> _userManager;

        public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(EjecutarUsuarioRolAgregar request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(request.RolNombre);
            if (role == null)
                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El rol no existe" });

            var usuarioIden = await _userManager.FindByNameAsync(request.Username);
            if (usuarioIden == null)
                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El usuario no existe" });

            var resultado = await _userManager.AddToRoleAsync(usuarioIden, request.RolNombre);
            if (resultado.Succeeded) return Unit.Value;

            throw new Exception("No se pudo agregar el Rol al usuario");
        }
    }
}