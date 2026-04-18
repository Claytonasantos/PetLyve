using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetLyve.Domain.Entities;

namespace PetLyve.Infrastructure.Data.Mappings;

public class AnimalMapping : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable("Animais");

        builder.HasKey(a => a.AnimalId);

        builder.Property(a => a.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Especie)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasMany(a => a.Agendamentos)
            .WithOne(ag => ag.Animal)
            .HasForeignKey(ag => ag.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}