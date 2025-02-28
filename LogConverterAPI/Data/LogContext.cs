using Microsoft.EntityFrameworkCore;
using LogConverterAPI.Models;

namespace LogConverterAPI.Data
{
    public class LogContext : DbContext
    {
        public DbSet<LogOriginal> LogsOriginais { get; set; }
        public DbSet<LogTransformado> LogsTransformados { get; set; }

        public LogContext(DbContextOptions<LogContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Adiciona um índice único na coluna Conteudo da tabela LogsOriginais
            modelBuilder.Entity<LogOriginal>()
                .HasIndex(l => l.Id)
                .IsUnique();

            // Configura a relação entre LogOriginal e LogTransformado
            modelBuilder.Entity<LogTransformado>()
                .HasOne<LogOriginal>()
                .WithMany()
                .HasForeignKey(t => t.LogOriginalId);
        }
    }
}
