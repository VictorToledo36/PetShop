using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class ServicoPrecoConfiguration : EntidadeBaseConfiguration<ServicoPreco>
{
    public override void Configure(EntityTypeBuilder<ServicoPreco> builder)
    {
        base.Configure(builder);

        builder.ToTable("ServicoPrecos");

        builder.HasIndex(sp => new { sp.ServicoId, sp.Porte }).IsUnique();

        builder.Property(sp => sp.Porte).IsRequired().HasConversion<string>().HasMaxLength(20);

        builder.Property(sp => sp.Preco).IsRequired().HasColumnType("decimal(10,2)");

        builder.HasOne(sp => sp.Servico).WithMany(s => s.Precos).HasForeignKey(sp => sp.ServicoId).OnDelete(DeleteBehavior.Cascade);
    }
}