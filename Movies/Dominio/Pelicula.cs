using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio;

public class Pelicula
{
    [Key] public int PeliculaID { get; set; }

    public string Titulo { get; set; }

    public string Descripcion { get; set; }

    public int CostoAlquiler { get; set; }

    public int CantidadPeliculas { get; set; }

    [Column(TypeName = "decimal(18,4)")] public decimal Estrellas { get; set; }

    public string URLPelicula { get; set; }


    [NotMapped] public byte[] FotoPelicula { get; set; }

    [DefaultValue("true")] public bool Disponible { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Lanzamiento { get; set; }

    public ICollection<PeliculaAlquiler> PeliculaAlquiler { get; set; }
    public ICollection<PeliculaActor> Actorlnk { get; set; }
    public ICollection<PeliculaDirector> DirectorLnk { get; set; }
}