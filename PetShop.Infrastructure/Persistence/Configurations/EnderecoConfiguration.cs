using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class EnderecoConfiguration : EntidadeBaseConfiguration<Endereco>
{
    public override void Configure(EntityTypeBuilder<Endereco> builder)
    {
        base.Configure(builder);

        builder.ToTable("Enderecos");

        builder.Property(e => e.CEP).IsRequired().HasMaxLength(8); 

        builder.Property(e => e.Logradouro).IsRequired().HasMaxLength(200);

        builder.Property(e => e.Numero).IsRequired().HasMaxLength(20);

        builder.Property(e => e.Complemento).HasMaxLength(100);

        builder.Property(e => e.Bairro).IsRequired().HasMaxLength(100);

        builder.Property(e => e.Cidade).IsRequired().HasMaxLength(100);

        builder.Property(e => e.Estado).IsRequired().HasMaxLength(2); 

        builder.HasOne<ApplicationUser>().WithOne(u => u.Endereco).HasForeignKey<Endereco>(e => e.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
    }
}