using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class AgendamentoConfiguration : EntidadeBaseConfiguration<Agendamento>
{
    public override void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        base.Configure(builder);

        builder.ToTable("Agendamentos");

        builder.Property(a => a.ApplicationUserId).IsRequired().HasMaxLength(450);

        builder.Property(a => a.DataSolicitada).IsRequired();

        builder.Property(a => a.HorarioSolicitado).IsRequired().HasColumnType("time");

        builder.Property(a => a.Status).IsRequired().HasConversion<string>().HasMaxLength(50);

        builder.Property(a => a.ObservacaoCliente).HasMaxLength(500);

        builder.Property(a => a.MotivoRecusa).HasMaxLength(500);

        builder.Property(a => a.DataResposta).IsRequired(false);

        builder.HasOne(a => a.Pet).WithMany(p => p.Agendamentos).HasForeignKey(a => a.PetId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Servico).WithMany(s => s.Agendamentos).HasForeignKey(a => a.ServicoId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>().WithMany(u => u.Agendamentos).HasForeignKey(a => a.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
    }
}