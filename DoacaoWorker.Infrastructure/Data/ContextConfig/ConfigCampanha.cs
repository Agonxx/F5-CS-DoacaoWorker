using DoacaoWorker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoacaoWorker.Infrastructure.Data.ContextConfig
{
    public class ConfigCampanha : IEntityTypeConfiguration<Campanha>
    {
        public void Configure(EntityTypeBuilder<Campanha> builder)
        {
            builder.ToTable("Campanhas");
            builder.Property(c => c.ValorArrecadado).HasPrecision(18, 2);
        }
    }
}
