using DoacaoWorker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoacaoWorker.Infrastructure.Data.ContextConfig
{
    public class ConfigDoacao : IEntityTypeConfiguration<Doacao>
    {
        public void Configure(EntityTypeBuilder<Doacao> builder)
        {
            builder.ToTable("Doacoes");
            builder.Property(d => d.ValorDoacao).HasPrecision(18, 2);
        }
    }
}
