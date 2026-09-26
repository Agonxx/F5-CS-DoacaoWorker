using DoacaoWorker.Domain.Entities;
using DoacaoWorker.Infrastructure.Data.ContextConfig;
using Microsoft.EntityFrameworkCore;

namespace DoacaoWorker.Infrastructure.Data
{
    // Aponta para o CampanhasDB. Sem EnsureCreated/migrations: o schema é da CampanhasApi
    public class DoacaoWorkerDbContext : DbContext
    {
        public DoacaoWorkerDbContext(DbContextOptions<DoacaoWorkerDbContext> options) : base(options)
        {
        }

        public DbSet<Campanha> Campanhas { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigCampanha());
            modelBuilder.ApplyConfiguration(new ConfigDoacao());
        }
    }
}
