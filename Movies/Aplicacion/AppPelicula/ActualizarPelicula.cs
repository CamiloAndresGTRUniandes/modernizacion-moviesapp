using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.AppPelicula;

public class ActualizarPelicula
{
    public class ActualizarPeliculaParametros : IRequest<ResponseOperations>
    {
        public int PeliculaID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int CostoAlquiler { get; set; }
        public int CantidadPeliculas { get; set; }
        public decimal Estrellas { get; set; }
        public string urlPelicula { get; set; }
        public DateTime Lanzamiento { get; set; }
        public bool Disponible { get; set; }

        public ICollection<PeliculaActor> Actorlnk { get; set; }
        public ICollection<PeliculaDirector> DirectorLnk { get; set; }
        public object Data { get; set; }
    }

    public class EjecutaValidacion : AbstractValidator<ActualizarPeliculaParametros>
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

    public class Manejador : IRequestHandler<ActualizarPeliculaParametros, ResponseOperations>
    {
        private readonly VideoBlockOnlineContext _context;

        public Manejador(VideoBlockOnlineContext context)
        {
            _context = context;
        }

        public async Task<ResponseOperations> Handle(ActualizarPeliculaParametros request,
            CancellationToken cancellationToken)
        {
            var pelicula = await _context.Pelicula.FindAsync(request.PeliculaID);
            if (pelicula == null)
                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro la pelicula" });


            pelicula.Titulo = request.Titulo;
            pelicula.Descripcion = request.Descripcion;
            pelicula.CostoAlquiler = request.CostoAlquiler;
            pelicula.CantidadPeliculas = request.CantidadPeliculas;
            pelicula.Estrellas = request.Estrellas;
            pelicula.URLPelicula = request.urlPelicula;
            pelicula.Lanzamiento = request.Lanzamiento;
            pelicula.Disponible = request.Disponible;
            var FileName = request.urlPelicula;


            /*actualizar el actor de la pelicula*/


            if (request.Actorlnk != null)
                if (request.Actorlnk.Count > 0)
                {
                    /*Eliminar los actores actuales de la pelicula en la base de datos*/
                    var ActorBD = _context.PeliculaActor.Where(x => x.PeliculaID == request.PeliculaID);
                    foreach (var ActorEliminar in ActorBD) _context.PeliculaActor.Remove(ActorEliminar);

                    /*Fin del procedimiento para eliminar actores*/
                    /*Procedimiento para agregar actores que provienen del cliente*/
                    foreach (var id in request.Actorlnk)
                    {
                        var peliculaActor = new PeliculaActor
                        {
                            ActorDirectorID = id.ActorDirectorID,
                            PeliculaID = pelicula.PeliculaID
                        };
                        _context.PeliculaActor.Add(peliculaActor);
                    }
                    /*Fin del procedimiento*/
                }


            /*actualizar los  directores de la pelicula*/


            if (request.DirectorLnk != null)
                if (request.DirectorLnk.Count > 0)
                {
                    /*Eliminar los directores actuales de la pelicula en la base de datos*/
                    var DirectorBD = _context.PeliculaDirector.Where(x => x.PeliculaID == request.PeliculaID);
                    foreach (var DirectorEliminar in DirectorBD) _context.PeliculaDirector.Remove(DirectorEliminar);

                    /*Fin del procedimiento para eliminar actores*/
                    /*Procedimiento para agregar actores que provienen del cliente*/
                    foreach (var id in request.DirectorLnk)
                    {
                        var peliculaDirector = new PeliculaDirector
                        {
                            ActorDirectorID = id.ActorDirectorID,
                            PeliculaID = pelicula.PeliculaID
                        };
                        _context.PeliculaDirector.Add(peliculaDirector);
                    }
                    /*Fin del procedimiento*/
                }


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
                    pelicula.URLPelicula = nuevoArchivo;
                }
                else
                {
                    FileName = request.urlPelicula;
                }
            }
            catch
            {
                FileName = request.urlPelicula;
            }

            var resultado = await _context.SaveChangesAsync();
            return new ResponseOperations
                { Ok = true, Message = "La pelicula fue actualizada", Id = pelicula.PeliculaID };
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