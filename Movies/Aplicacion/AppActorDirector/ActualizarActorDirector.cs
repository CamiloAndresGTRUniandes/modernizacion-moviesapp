using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.AppActorDirector;

public class ActualizarActorDirector
{
    public class ParametrosActorDirector : IRequest<ResponseOperations>
    {
        public int ActorDirectorID { get; set; }
        public string Nombre { get; set; }
        public bool EsActor { get; set; }
        public bool EsDirector { get; set; }
        public string urlFoto { get; set; }
        public object Data { get; set; }
    }

    public class Validacion : AbstractValidator<ParametrosActorDirector>
    {
        public Validacion()
        {
            RuleFor(x => x.Nombre).NotEmpty();
        }
    }

    public class Manejador : IRequestHandler<ParametrosActorDirector, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }


        public async Task<ResponseOperations> Handle(ParametrosActorDirector request,
            CancellationToken cancellationToken)
        {
            try
            {
                var FileName = request.urlFoto;
                var ActorDirector = await _context.ActorDirector.FindAsync(request.ActorDirectorID);
                if (ActorDirector == null)
                    return new ResponseOperations { Ok = false, Message = "No se encontro el actor director ", Id = 0 };

                ActorDirector.Nombre = request.Nombre;
                ActorDirector.EsActor = request.EsActor;
                ActorDirector.EsDirector = request.EsDirector;
                ActorDirector.urlFoto = request.urlFoto;

                try
                {
                    if (request.Data != null)
                    {
                        var files = request.Data.ToString().Replace("data:image/png;base64,", string.Empty);
                        ;
                        files = files.Replace("data:image/jpg;base64,", string.Empty);
                        files = files.Replace("data:image/jpeg;base64,", string.Empty);
                        var Contenido = Convert.FromBase64String(files);

                        var nuevoArchivo = Guid.NewGuid() + ".jpg";
                        byteArrayToImage(Contenido, nuevoArchivo);
                        ActorDirector.urlFoto = nuevoArchivo;
                    }
                    else
                    {
                        FileName = request.urlFoto;
                    }
                }
                catch
                {
                    FileName = request.urlFoto;
                }


                var valor = await _context.SaveChangesAsync();


                return new ResponseOperations
                    { Ok = true, Message = "El actor director fue actualizado", Id = ActorDirector.ActorDirectorID };
            }
            catch (Exception e)
            {
                return new ResponseOperations { Ok = false, Message = e.Message, Id = 0 };
            }
        }

        public Image byteArrayToImage(byte[] bytesArr, string archivo)
        {
            using (var memstr = new MemoryStream(bytesArr))
            {
                var img = Image.FromStream(memstr);


                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/actoresdirectores", archivo);
                img.Save(path);


                return img;
            }
        }
    }
}