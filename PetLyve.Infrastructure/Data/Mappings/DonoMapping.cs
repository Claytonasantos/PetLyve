using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetLyve.Domain.Entities;

namespace PetLyve.Infrastructure.Data.Mappings;

public class DonoMapping : IEntityTypeConfiguration<Dono>
{
    public void Configure(EntityTypeBuilder<Dono> builder)
    {
        builder.ToTable("Donos");
        
        builder.HasKey(d => d.DonoId);
        
        builder.Property(d => d.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Email)
            .HasMaxLength(100);
        
        builder.HasMany(d => d.Animais)
            .WithOne(a => a.Dono)
            .HasForeignKey(a => a.DonoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}