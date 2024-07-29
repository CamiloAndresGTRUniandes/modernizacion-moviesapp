using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio;

[NotMapped]
public class ResponseOperations
{
    public bool Ok { get; set; }
    public string Message { get; set; }
    public int Id { get; set; }
}