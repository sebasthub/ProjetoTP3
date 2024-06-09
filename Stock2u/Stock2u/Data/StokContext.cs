using Microsoft.EntityFrameworkCore;
using Stock2u.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Stock2u.Data
{
    public class StokContext : IdentityDbContext<IdentityUser>
    {
        public StokContext(DbContextOptions<StokContext> options): base(options) { 
        }

        public DbSet<EstoqueRestaurante> EstoqueRestaurantes { get; set; } = null!;
        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<Retirada> Retiradas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<EstoqueRestaurante>()
                .HasMany(e => e.Produtos)
                .WithOne(e => e.EstoqueRestaurante)
                .HasForeignKey(e => e.IDEstoqueRestaurante)
                .HasPrincipalKey(e => e.ID);

            modelBuilder.Entity<Retirada>()
                .HasOne(e => e.Produto)
                .WithMany()
                .HasForeignKey(e => e.ID);

            base.OnModelCreating(modelBuilder);

        }
    }
}
