using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class ConfiguracaoAgendaConfiguration : EntidadeBaseConfiguration<ConfiguracaoAgenda>
{
    public override void Configure(EntityTypeBuilder<ConfiguracaoAgenda> builder)
    {
        base.Configure(builder);

        builder.ToTable("ConfiguracoesAgenda");

        builder.Property(c => c.DuracaoSlotMinutos).IsRequired();

        builder.Property(c => c.DiasAntecedenciaMaxima).IsRequired();

        builder.HasData(new ConfiguracaoAgenda
        {
            Id = 1,
            DuracaoSlotMinutos = 30,
            DiasAntecedenciaMaxima = 30,
            DataCriacao = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Ativo = true
        });
    }
}
