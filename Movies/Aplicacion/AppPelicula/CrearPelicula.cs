using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.AppPelicula;

public class CrearPelicula
{
    public class CreacionPeliculaParametros : IRequest<ResponseOperations>
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int CostoAlquiler { get; set; }
        public int CantidadPeliculas { get; set; }
        public decimal Estrellas { get; set; }
        public string URLPelicula { get; set; }
        public DateTime Lanzamiento { get; set; }
        public bool Disponible { get; set; }
        public object Data { get; set; }

        public ICollection<PeliculaActor> Actorlnk { get; set; }
        public ICollection<PeliculaDirector> DirectorLnk { get; set; }
    }

    public class EjecutaValidacion : AbstractValidator<CreacionPeliculaParametros>
    {
        public EjecutaValidacion()
        {
            RuleFor(x => x.Titulo).NotEmpty();
            RuleFor(x => x.Descripcion).NotEmpty();
            RuleFor(x => x.Lanzamiento).NotEmpty();
            RuleFor(x => x.CostoAlquiler).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CantidadPeliculas).GreaterThanOrEqualTo(1);
            RuleFor(x => x.CantidadPeliculas).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Estrellas).GreaterThanOrEqualTo(0);
        }
    }

    public class Manejador : IRequestHandler<CreacionPeliculaParametros, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<ResponseOperations> Handle(CreacionPeliculaParametros request,
            CancellationToken cancellationToken)
        {
            var pelicula = new Pelicula
            {
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                CostoAlquiler = request.CostoAlquiler,
                CantidadPeliculas = request.CantidadPeliculas,
                Estrellas = request.Estrellas,
                URLPelicula = request.URLPelicula,
                Lanzamiento = request.Lanzamiento,
                Disponible = request.Disponible
            };

            _context.Pelicula.Add(pelicula);
            var resultado = await _context.SaveChangesAsync();

            if (request.Actorlnk != null)
                foreach (var id in request.Actorlnk)
                {
                    var peliculaActor = new PeliculaActor
                    {
                        ActorDirectorID = id.ActorDirectorID,
                        PeliculaID = pelicula.PeliculaID
                    };
                    _context.PeliculaActor.Add(peliculaActor);
                }

            if (request.DirectorLnk != null)
                foreach (var id in request.DirectorLnk)
                {
                    var PeliculaDirector = new PeliculaDirector
                    {
                        ActorDirectorID = id.ActorDirectorID,
                        PeliculaID = pelicula.PeliculaID
                    };
                    _context.PeliculaDirector.Add(PeliculaDirector);
                }


            var FileName = Guid.NewGuid() + ".jpg";
            try
            {
                if (request.Data != null)
                {
                    var files = request.Data.ToString().Replace("data:image/png;base64,", string.Empty);
                    ;
                    files = files.Replace("data:image/jpg;base64,", string.Empty);
                    files = files.Replace("data:image/jpeg;base64,", string.Empty);
                    var Contenido = Convert.FromBase64String(files);
                    byteArrayToImage(Contenido, FileName);
                }
                else
                {
                    FileName = "noImage.png";
                }
            }
            catch
            {
                FileName = "noImage.png";
            }

            pelicula.URLPelicula = FileName;

            //  var valor = await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();

            if (resultado > 0)
                return new ResponseOperations
                    { Ok = true, Message = "La pelicula fue creada", Id = pelicula.PeliculaID };
            return new ResponseOperations { Ok = false, Message = "La pelicula no fue creada ", Id = 0 };

            throw new Exception("No se pudo insertar la pelicula");
        }

        public Image byteArrayToImage(byte[] bytesArr, string archivo)
        {
            using (var memstr = new MemoryStream(bytesArr))
            {
                var img = Image.FromStream(memstr);


                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/peliculas", archivo);
                img.Save(path);


                return img;
            }
        }
    }
}