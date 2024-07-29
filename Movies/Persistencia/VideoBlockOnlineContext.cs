using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia;

public class VideoBlockOnlineContext : IdentityDbContext<Usuario>
{
    public VideoBlockOnlineContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Documento> Documento { get; set; }
    public DbSet<ActorDirector> ActorDirector { get; set; }
    public DbSet<EstadoAlquiler> EstadoAlquiler { get; set; }
    public DbSet<Pelicula> Pelicula { get; set; }
    public DbSet<PeliculaActor> PeliculaActor { get; set; }
    public DbSet<PeliculaAlquiler> PeliculaAlquiler { get; set; }
    public DbSet<PeliculaDirector> PeliculaDirector { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<PeliculaActor>().HasKey(ci => new { ci.PeliculaID, ci.ActorDirectorID });
        modelBuilder.Entity<PeliculaDirector>().HasKey(ci => new { ci.PeliculaID, ci.ActorDirectorID });
    }
}