using System;
using System.Collections.Generic;
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

public class Login
{
    public class EjecutaLogin : IRequest<UsuarioData>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class EjecutaValidacion : AbstractValidator<EjecutaLogin>
    {
        public EjecutaValidacion()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class Manejador : IRequestHandler<EjecutaLogin, UsuarioData>
    {
        private readonly VideoBlockOnlineContext _context;
        private readonly IJwtGenerador _jwtGenerador;
        private readonly SignInManager<Usuario> _signInManager;

        private readonly UserManager<Usuario> _userManager;

        public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager,
            IJwtGenerador jwtGenerador, VideoBlockOnlineContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerador = jwtGenerador;
            _context = context;
        }

        public async Task<UsuarioData> Handle(EjecutaLogin request, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null) throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
            var resultadoRoles = await _userManager.GetRolesAsync(usuario);
            var listaRoles = new List<string>(resultadoRoles);


            var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuario.Id))
                .FirstOrDefaultAsync();

            if (resultado.Succeeded)
            {
                if (imagenPerfil != null)
                {
                    var imagenCliente = new ImagenGeneral
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        ImagenPerfil = imagenCliente
                    };
                }

                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                    Username = usuario.UserName,
                    Email = usuario.Email,
                    Imagen = null
                };
            }


            throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
        }
    }
}