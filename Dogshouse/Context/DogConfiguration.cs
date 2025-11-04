using Dogshouse.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dogshouse.Context;

public class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.ToTable("Dogs");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(50);

        builder.HasIndex(d => d.Name)
                .IsUnique();

        builder.Property(d => d.Color)
                .IsRequired()
                .HasMaxLength(50);

        builder.Property(d => d.TailLength)
                .IsRequired();

        builder.Property(d => d.Weight)
                 .IsRequired();
    }
}

