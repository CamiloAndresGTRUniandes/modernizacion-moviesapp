using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad;

public class Registrar
{
    public class EjecutaRegistrar : IRequest<UsuarioData>
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Username { get; set; }
    }

    public class EjecutaValidador : AbstractValidator<EjecutaRegistrar>
    {
        public EjecutaValidador()
        {
            RuleFor(x => x.Nombre).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            ///RuleFor(x => x.Username).NotEmpty();
        }
    }


    public class Manejador : IRequestHandler<EjecutaRegistrar, UsuarioData>
    {
        private readonly VideoBlockOnlineContext _context;
        private readonly IJwtGenerador _jwtGenerador;
        private readonly UserManager<Usuario> _userManager;

        public Manejador(VideoBlockOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
        {
            _context = context;
            _userManager = userManager;
            _jwtGenerador = jwtGenerador;
        }

        public async Task<UsuarioData> Handle(EjecutaRegistrar request, CancellationToken cancellationToken)
        {
            request.Username = request.Email;
            var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
            if (existe)
                throw new ManejadorExcepcion(HttpStatusCode.BadRequest,
                    new { mensaje = "Existe ya un usuario registrado con este email" });

            var existeUserName = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();
            if (existeUserName)
                throw new ManejadorExcepcion(HttpStatusCode.BadRequest,
                    new { mensaje = "Existe ya un usuario con este username" });


            var usuario = new Usuario
            {
                NombreCompleto = request.Nombre,
                Email = request.Email,
                UserName = request.Username
            };

            var resultado = await _userManager.CreateAsync(usuario, request.Password);
            if (resultado.Succeeded)
            {
                var usuarioIden = await _userManager.FindByNameAsync(request.Username);
                await _userManager.AddToRoleAsync(usuarioIden, "Cliente");
                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Token = _jwtGenerador.CrearToken(usuario, null),
                    Username = usuario.UserName,
                    Email = usuario.Email
                };
            }


            throw new Exception("No se pudo agregar al nuevo usuario");
        }
    }
}