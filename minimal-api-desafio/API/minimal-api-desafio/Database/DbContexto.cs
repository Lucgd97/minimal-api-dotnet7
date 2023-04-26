using Microsoft.EntityFrameworkCore;
using MinimalApiDesafio.Models;

namespace minimal_api_desafio.Database;

public class DbContexto : DbContext
{
    // Entities
    public DbContexto(DbContextOptions<DbContexto> options) : base(options) {}
    public DbSet<Cliente> Clientes { get; set; } = default!;
}