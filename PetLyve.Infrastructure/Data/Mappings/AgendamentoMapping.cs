using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetLyve.Domain.Entities;

namespace PetLyve.Infrastructure.Data.Mappings;

public class AgendamentoMapping : IEntityTypeConfiguration<Agendamento>
{
    public void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        builder.HasKey(a => a.AgendamentoId);
        
        builder.HasOne(a => a.Pagamento)
            .WithOne(p => p.Agendamento)
            .HasForeignKey<Pagamento>(p => p.AgendamentoId)
            .IsRequired(false);
        
        builder.HasOne(a => a.Animal)
            .WithMany(an => an.Agendamentos)
            .HasForeignKey(a => a.AnimalId);

        builder.HasOne(a => a.Funcionario)
            .WithMany(f => f.Agendamentos)
            .HasForeignKey(a => a.FuncionarioId);

        builder.HasOne(a => a.Servico)
            .WithMany(s => s.Agendamentos)
            .HasForeignKey(a => a.ServicoId);
    }
}