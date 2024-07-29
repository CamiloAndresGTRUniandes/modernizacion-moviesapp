using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia;

public class DataPrueba
{
    public static async Task InsertarData(VideoBlockOnlineContext context,
        UserManager<Usuario> usuarioManager,
        RoleManager<IdentityRole> _roleManager
    )
    {
        if (!usuarioManager.Users.Any())
        {
            var usuario = new Usuario
                { NombreCompleto = "Yonathan Beltran", UserName = "ybeltranr", Email = "nathanbelt23@gmail.com" };
            await usuarioManager.CreateAsync(usuario, "Password123$");
        }

        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "Cliente" });
        }

     
    }
}