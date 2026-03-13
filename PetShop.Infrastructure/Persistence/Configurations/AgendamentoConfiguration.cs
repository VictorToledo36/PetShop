using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.ToTable("Agendamentos");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.ApplicationUserId).HasMaxLength(450); 

            builder.Property(a => a.DataSolicitada).IsRequired();

            builder.Property(a => a.HorarioSolicitado).IsRequired().HasColumnType("time");

            builder.Property(a => a.Status).IsRequired().HasConversion<string>();

            builder.Property(a => a.ObservacaoCliente).HasMaxLength(500);

            builder.Property(a => a.MotivoRecusa).HasMaxLength(500);

            builder.Property(a => a.DataResposta).IsRequired(false);

            builder.HasOne(a => a.Pet).WithMany().HasForeignKey(a => a.PetId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Servico).WithMany().HasForeignKey(a => a.ServicoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}