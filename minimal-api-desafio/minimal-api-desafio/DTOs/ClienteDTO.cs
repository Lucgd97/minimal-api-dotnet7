namespace MinimalApiDesafio.DTOs;

public record ClienteDTO
{
    public int Id {get; set;}
    public string? Nome {get; set;}
    public string? Telefone {get; set;}
    public string? Email {get; set;}
}