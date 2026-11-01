using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class MensagemAgendamentoConfiguration : EntidadeBaseConfiguration<MensagemAgendamento>
{
    public override void Configure(EntityTypeBuilder<MensagemAgendamento> builder)
    {
        base.Configure(builder);

        builder.ToTable("MensagensAgendamento");

        builder.Property(m => m.ApplicationUserId).IsRequired().HasMaxLength(450);

        builder.Property(m => m.Texto).IsRequired().HasMaxLength(1000);

        builder.Property(m => m.EnviadaPeloAdmin).IsRequired();

        builder.Property(m => m.Lida).IsRequired().HasDefaultValue(false);

        builder.HasOne(m => m.Agendamento)
            .WithMany(a => a.Mensagens)
            .HasForeignKey(m => m.AgendamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(m => m.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
