namespace Dominio;

public class PeliculaDirector
{
    public int ActorDirectorID { get; set; }
    public int PeliculaID { get; set; }

    public ActorDirector ActorDirector { get; set; }
    public Pelicula Pelicula { get; set; }
}