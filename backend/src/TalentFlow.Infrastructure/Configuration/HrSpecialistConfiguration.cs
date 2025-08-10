using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Infrastructure.Configuration;

public class HrSpecialistConfiguration : IEntityTypeConfiguration<HrSpecialist>
{
    public void Configure(EntityTypeBuilder<HrSpecialist> builder)
    {
        builder.ToTable("hr_specialist");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id)
            .HasConversion(
                id => id.Value,
                result => HrSpecialistId.Create(result)
            );

        builder.ComplexProperty(a => a.FullName, fb =>
        {
            fb.Property(a => a!.FirstName).IsRequired(false).HasColumnName("first_name");
            fb.Property(a => a!.SecondName).IsRequired(false).HasColumnName("second_name");
        });

        builder.ComplexProperty(w => w.ContactInfo,
            db =>
            {
                db.Property(p => p.Email)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_100)
                    .HasColumnName("email");
                db.Property(p => p.Phone)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_25)
                    .HasColumnName("phone");
            }
        );

        builder.HasMany(v => v.AssignedVacancies)
            .WithOne()
            .HasForeignKey(v => v.HrSpecialistId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}