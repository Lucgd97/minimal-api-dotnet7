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

    [Required]
    [MaxLength(100)]
    public string? Nome {get; set;} 

    [Required]
    [MaxLength(20)]
    public string? Telefone {get; set;} 

    [MaxLength(200)]
    public string? Email {get; set;}
    
    public DateTime DataCriacao { get;set; }
}