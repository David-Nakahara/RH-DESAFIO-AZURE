using Microsoft.EntityFrameworkCore;
using RH_Azure.Models;

namespace RH_Azure.Context
{
    public class RHContext : DbContext
    {
        public RHContext(DbContextOptions<RHContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionarios { get; set; }
    }
}
