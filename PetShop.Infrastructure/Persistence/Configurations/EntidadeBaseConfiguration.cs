using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShop.Domain.Entities;

namespace PetShop.Infrastructure.Persistence.Configurations;

public abstract class EntidadeBaseConfiguration<T> : IEntityTypeConfiguration<T>
    where T : EntidadeBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.DataCriacao).IsRequired().HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.Ativo).IsRequired().HasDefaultValue(true);
    }
}