using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public class PetConfiguration : EntidadeBaseConfiguration<Pet>
{
    public override void Configure(EntityTypeBuilder<Pet> builder)
    {
        base.Configure(builder);

        builder.ToTable("Pets");

        builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);

        builder.Property(p => p.Raca).HasMaxLength(100);

        builder.Property(p => p.Peso).HasColumnType("decimal(5,2)");

        builder.Property(p => p.Porte).IsRequired().HasConversion<string>().HasMaxLength(20);

        builder.Property(p => p.Observacao).HasMaxLength(500);

        builder.HasOne<ApplicationUser>().WithMany(u => u.Pets).HasForeignKey(p => p.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
    }
}