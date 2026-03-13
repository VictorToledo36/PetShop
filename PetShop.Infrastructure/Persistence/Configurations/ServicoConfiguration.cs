using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class ServicoConfiguration : EntidadeBaseConfiguration<Servico>
{
    public override void Configure(EntityTypeBuilder<Servico> builder)
    {
        base.Configure(builder);

        builder.ToTable("Servicos");

        builder.Property(s => s.Nome).IsRequired().HasMaxLength(100);

        builder.Property(s => s.Descricao).HasMaxLength(500);
    }
}