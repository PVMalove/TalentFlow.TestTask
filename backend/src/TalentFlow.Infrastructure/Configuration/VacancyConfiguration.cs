using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Enums;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Infrastructure.Configuration;

public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.ToTable("vacancies");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                result => VacancyId.Create(result)
            );

        builder.Property(v => v.Status)
            .HasConversion(
                s => s.ToString(),
                s => (VacancyStatus)Enum.Parse(typeof(VacancyStatus), s))
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_25);

        builder.Property(v => v.Title)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_100);

        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH_2000);

        builder.Property(v => v.OpeningDate)
            .IsRequired();
    }
}