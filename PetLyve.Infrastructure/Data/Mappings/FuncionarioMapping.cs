using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetLyve.Domain.Entities;

namespace PetLyve.Infrastructure.Data.Mappings;

public class FuncionarioMapping : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.ToTable("Funcionarios");

        builder.HasKey(f => f.FuncionarioId);

        builder.Property(f => f.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Cargo)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasMany(f => f.Agendamentos)
            .WithOne(ag => ag.Funcionario)
            .HasForeignKey(ag => ag.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}