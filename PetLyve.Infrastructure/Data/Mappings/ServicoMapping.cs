using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetLyve.Domain.Entities;

namespace PetLyve.Infrastructure.Data.Mappings;

public class ServicoMapping : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos");

        builder.HasKey(s => s.ServicoId);

        builder.Property(s => s.NomeServico)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.HasMany(s => s.Agendamentos)
            .WithOne(ag => ag.Servico)
            .HasForeignKey(ag => ag.ServicoId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}