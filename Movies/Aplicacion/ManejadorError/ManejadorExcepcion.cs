using System;
using System.Net;

namespace Aplicacion.ManejadorError;

public class ManejadorExcepcion : Exception
{
    public ManejadorExcepcion(HttpStatusCode codigo, object errores = null)
    {
        Codigo = codigo;
        Errores = errores;
    }

    public HttpStatusCode Codigo { get; }
    public object Errores { get; }
}