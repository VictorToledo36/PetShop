using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class BloqueioAgendaConfiguration : EntidadeBaseConfiguration<BloqueioAgenda>
{
    public override void Configure(EntityTypeBuilder<BloqueioAgenda> builder)
    {
        base.Configure(builder);

        builder.ToTable("BloqueiosAgenda");

        builder.Property(b => b.Data).IsRequired();

        builder.Property(b => b.Horario).HasColumnType("time");

        builder.Property(b => b.Motivo).HasMaxLength(200);
    }
}
