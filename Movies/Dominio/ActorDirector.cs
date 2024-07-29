using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio;

public class ActorDirector
{
    [Key] public int ActorDirectorID { get; set; }

    public string Nombre { get; set; }

    public bool EsActor { get; set; }
    public bool EsDirector { get; set; }

    public string? urlFoto { get; set; }

    [NotMapped] public byte[] FotoActorDirector { get; set; }

    [NotMapped] public bool Seleccionado { get; set; }


    private ICollection<PeliculaActor> Peliculalnk { get; set; }
    private ICollection<PeliculaDirector> PeliculaDirectorlnk { get; set; }
}