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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}