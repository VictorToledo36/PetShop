using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class HorarioFuncionamentoConfiguration : EntidadeBaseConfiguration<HorarioFuncionamento>
{
    public override void Configure(EntityTypeBuilder<HorarioFuncionamento> builder)
    {
        base.Configure(builder);

        builder.ToTable("HorariosFuncionamento");

        builder.Property(h => h.DiaSemana).IsRequired();

        builder.Property(h => h.HoraInicio).IsRequired().HasColumnType("time");

        builder.Property(h => h.HoraFim).IsRequired().HasColumnType("time");
    }
}
