using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApiDesafio.Models;

[Table("clientes")]
public record Cliente
{
    public Cliente()
    {
        this.DataCriacao = DateTime.Now;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    public string Nome {get; set;} = default!;

    [MaxLength(20)]
    public string Telefone {get; set;} = default!;

    public string? Email {get; set;}
    
    public DateTime DataCriacao { get;set; }
}