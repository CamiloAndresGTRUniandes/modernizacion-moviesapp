using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad;

public class UsuarioActual
{
    public class Ejecutar : IRequest<UsuarioData>
    {
    }

    public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
    {
        private readonly VideoBlockOnlineContext _context;
        private readonly IJwtGenerador _jwtGenerador;
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioSesion _usuarioSesion;

        public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion,
            VideoBlockOnlineContext context)
        {
            _userManager = userManager;
            _jwtGenerador = jwtGenerador;
            _usuarioSesion = usuarioSesion;
            _context = context;
        }

        public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            var resultadoRoles = await _userManager.GetRolesAsync(usuario);
            var listaRoles = new List<string>(resultadoRoles);

            var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuario.Id))
                .FirstOrDefaultAsync();
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
                    Username = usuario.UserName,
                    Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                    Email = usuario.Email,
                    ImagenPerfil = imagenCliente
                };
            }

            return new UsuarioData
            {
                NombreCompleto = usuario.NombreCompleto,
                Username = usuario.UserName,
                Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                Email = usuario.Email
            };
        }
    }
}