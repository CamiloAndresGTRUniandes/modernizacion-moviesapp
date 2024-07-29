using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio;

public class PeliculaAlquiler
{
    [Key] public int PeliculaAlquilerID { get; set; }


    public string UserName { get; set; }

    [NotMapped] public string NombreUsuario { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime FechaAlquiler { get; set; }


    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    [Required(AllowEmptyStrings = true)]
    public DateTime FechaEntrega { get; set; }


    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? FechaDebeEntregar { get; set; }


    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]

    public DateTime? FechaCancelacion { get; set; }

    public int PeliculaID { get; set; }
    public Pelicula Pelicula { get; set; }
    public int EstadoAquilerID { get; set; }

    [ForeignKey("EstadoAquilerID")] public EstadoAlquiler EstadoAlquiler { get; set; }
}