using Microsoft.EntityFrameworkCore;
using MinimalApiDesafio.Models;

namespace minimal_api_desafio.Database;

public class DbContexto : DbContext
{
    // Entities
    public DbSet<Cliente> Clientes { get; set; } = default!;
}