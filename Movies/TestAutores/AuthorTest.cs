using Aplicacion.AppActorDirector;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GenFu;
using Dasync.Collections;

namespace TestAutores
{
    public class AuthorTest
    {
        VideoBlockOnlineContext _context;
        public AuthorTest()
        {
            var options = new DbContextOptionsBuilder<VideoBlockOnlineContext>()
     .UseInMemoryDatabase(databaseName: "FakeDatabase")
     .Options;

            _context = new VideoBlockOnlineContext(options);
        }




 
        private IEnumerable<ActorDirector> ObtenerDataPrueba()
        {

            // este metodo es para llenar con data de genfu
            A.Configure<ActorDirector>()
                .Fill(x => x.Nombre).AsArticleTitle();
                

            var lista = A.ListOf<ActorDirector>(30);
            lista[0].ActorDirectorID =1;

            return lista;
        }




        [Fact]
        public async void  GetActoresDirectores()
        {

            //1-Emulo context

            var context = "";
            context = Guid.NewGuid().ToString();



            var mockContexto = _context; ///new Mock<VideoBlockOnlineContext>(_context);
             _context.ActorDirector.Add(new ActorDirector() { ActorDirectorID = 1, EsActor = true, Nombre = "Robery" });

            _context.SaveChanges();


            ///  ConsultarActorDirector.Manejador manejador = new ConsultarActorDirector.Manejador(mockContexto.Object);
            ConsultarActorDirector.Manejador manejador = new ConsultarActorDirector.Manejador(mockContexto);
            ConsultarActorDirector.ParameterosConsultarActorDirector request = new ConsultarActorDirector.ParameterosConsultarActorDirector();
            request.Criterio = "a";
            request.Opcion = 0;
            

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any(), "Listado total");


        }
    }
}
