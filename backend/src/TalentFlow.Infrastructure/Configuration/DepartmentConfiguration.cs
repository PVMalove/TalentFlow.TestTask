using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Infrastructure.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                result => DepartmentId.Create(result)
            );
        
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_100)
            .HasColumnName("name");

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH_2000)
            .HasColumnName("description");
        
        builder.HasMany(d => d.Vacancies)
            .WithOne(v => v.Department)
            .HasForeignKey(v => v.DepartmentId);
    }
}